using Dapper;
using Newtonsoft.Json;
using PostorderCompany.Core.Events;
using PostorderCompany.Core.Infrastructure;
using PostorderCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;

namespace PostorderCompany.Order
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventHandler = new RabbitMQEventHandler("PostorderCompany.Order", HandleEvent);
            eventHandler.Start();

            Console.WriteLine("*** Order Service ***\n");
            Console.ReadKey(true);

            eventHandler.Stop();
        }

        private static bool HandleEvent(string eventType, string eventData)
        {
            bool handled = true;
            switch (eventType)
            {
                case "OrderOntvangen":
                    OrderOntvangen orderOntvangen = JsonConvert.DeserializeObject<OrderOntvangen>(eventData);
                    handled = Handle(orderOntvangen);
                    break;
                case "OrderIngepakt":
                    OrderIngepakt orderIngepakt = JsonConvert.DeserializeObject<OrderIngepakt>(eventData);
                    handled = Handle(orderIngepakt);
                    break;
                case "OrderBetaald":
                    OrderBetaald orderBetaald = JsonConvert.DeserializeObject<OrderBetaald>(eventData);
                    handled = Handle(orderBetaald);
                    break;
            }

            return handled;
        }

        private static bool Handle(OrderOntvangen orderOntvangen)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PostorderCompany"].ConnectionString))
            {
                string commandText = @"
                    INSERT INTO [dbo].[OrderStatus] ([orderId],[klant_naam],[klant_emailadres],
                        [klant_adres_straat],[klant_adres_huisnummer],[klant_adres_postcode],[klant_adres_plaats],[klant_adres_land],
                        [betaald],[ingepakt],[gewicht],[afmetingen])
                    VALUES (@orderId, @klantNaam,  @klantEmailadres,
                        @klantAdresStraat, @klantAdresHuisnummer, @klantAdresPostcode, @klantAdresPlaats, @klantAdresLand,
                        @betaald, @ingepakt, @gewicht, @afmetingen)";
                CommandDefinition cmd = new CommandDefinition(commandText, new OrderStatus() 
                {
                    orderId = orderOntvangen.orderId,
                    klantNaam = orderOntvangen.klant.naam,
                    klantEmailadres = orderOntvangen.klant.emailadres,
                    klantAdresStraat = orderOntvangen.klant.adres.straat,
                    klantAdresHuisnummer = orderOntvangen.klant.adres.huisnummer,
                    klantAdresPostcode = orderOntvangen.klant.adres.postcode,
                    klantAdresPlaats = orderOntvangen.klant.adres.plaats,
                    klantAdresLand = orderOntvangen.klant.adres.land
                });
                connection.Execute(cmd);
            }
            return true;
        }

        private static bool Handle(OrderIngepakt orderIngepakt)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PostorderCompany"].ConnectionString))
            {
                string commandText = @"
                    UPDATE [dbo].[OrderStatus] 
                    Set [ingepakt] = 1, 
                        [gewicht] = @gewicht,
                        [afmetingen] = @afmetingen
                    WHERE [orderId] = @orderId";
                CommandDefinition cmd = new CommandDefinition(commandText, orderIngepakt);
                connection.Execute(cmd);
            }
            verzendOrderWanneerBetaaldEnIngepakt(orderIngepakt.orderId);
            return true;
        }

        private static bool Handle(OrderBetaald orderBetaald)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PostorderCompany"].ConnectionString))
            {
                string commandText = @"
                    UPDATE [dbo].[OrderStatus] 
                    Set [betaald] = 1
                    WHERE [orderId] = @orderId";
                CommandDefinition cmd = new CommandDefinition(commandText, orderBetaald);
                connection.Execute(cmd);
            }
            verzendOrderWanneerBetaaldEnIngepakt(orderBetaald.orderId);
            return true;
        }

        private static void verzendOrderWanneerBetaaldEnIngepakt(string orderId) 
        {
            OrderStatus order = null;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PostorderCompany"].ConnectionString))
            {
                string commandText = @"
                    SELECT [orderId],[klant_naam],[klant_emailadres],
                        [klant_adres_straat],[klant_adres_huisnummer],[klant_adres_postcode],[klant_adres_plaats],[klant_adres_land],
                        [betaald],[ingepakt],[gewicht],[afmetingen] 
                    FROM [dbo].[OrderStatus]
                    WHERE [orderId] = @orderId";
                CommandDefinition cmd = new CommandDefinition(commandText, new { orderId = orderId });
                order = connection.Query<OrderStatus>(cmd).Single();
            }

            if (order.betaald && order.ingepakt)
            {
                var orderVerzonden = new OrderVerzonden()
                {
                    routingKey = "Order.Verzonden",
                    orderId = order.orderId,
                    ontvanger = new Persoonsgegevens()
                    {
                        naam = order.klantNaam,
                        emailadres = order.klantEmailadres,
                        adres = new Adres()
                        {
                            straat = order.klantAdresStraat,
                            huisnummer = order.klantAdresHuisnummer,
                            postcode = order.klantAdresPostcode,
                            plaats = order.klantAdresPlaats,
                            land = order.klantAdresLand
                        }
                    },
                    afzender = new Persoonsgegevens()
                    {
                        naam = "Postorder Company",
                        emailadres = "info@postorder-company.com",
                        adres = new Adres()
                        {
                            straat = "Singel",
                            huisnummer = "1a",
                            postcode = "1234AB",
                            plaats = "Amsterdam",
                            land = "Nederland"
                        }
                    },
                    gewicht = order.gewicht,
                    afmetingen = order.afmetingen
                };

                new RabbitMQEventPublisher().PublishEvent(orderVerzonden);
            }

        }

        private class OrderStatus
        {
            public string orderId { get; set; }
            public string klantNaam { get; set; }
            public string klantEmailadres { get; set; }
            public string klantAdresStraat { get; set; }
            public string klantAdresHuisnummer { get; set; }
            public string klantAdresPostcode { get; set; }
            public string klantAdresPlaats { get; set; }
            public string klantAdresLand { get; set; }
            public bool betaald { get; set; }
            public bool ingepakt { get; set; }
            public string gewicht { get; set; }
            public string afmetingen { get; set; }
        }

    }
}
