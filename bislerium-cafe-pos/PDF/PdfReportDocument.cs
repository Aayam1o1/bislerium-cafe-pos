using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using bislerium_cafe_pos.Models;
using Microsoft.Maui.Controls;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Colors = QuestPDF.Helpers.Colors;
using IContainer = QuestPDF.Infrastructure.IContainer;

namespace bislerium_cafe_pos.PDF
{
    public class PdfReportDocument : IDocument
    {
        public Report ReportObj;

        public PdfReportDocument(Report model)
        {
            ReportObj = model;
        }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {

            container
            .Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(10));
                page.Header().Element(Header);
                page.Content().Element(Content);
            });
        }
        void Header(IContainer container)
        {
            var pdfTitleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            string pdfTitleName = $"Bislerium Cafe {ReportObj.ReportType} Sales Transaction Report - ({ReportObj.ReportDate})";


            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"{pdfTitleName}").Style(pdfTitleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").Medium();
                        text.Span($"{DateTime.Now}").Medium();
                    });
                });
            });
        }
        void Content(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {

                var pdftitleStyle = TextStyle.Default.FontSize(16).SemiBold();

                string pdfTitleName = $"Top 5 Most Purchased Items - ({ReportObj.ReportDate})";

                column.Item().Text(pdfTitleName).Style(pdftitleStyle);

                column.Item().PaddingTop(7).Element(TableForMostPurchased);

                // Sales Transactions
                column.Item().PaddingTop(30).Element(SalesTransactionsHeader);
                column.Item().PaddingTop(10).Element(SalesTransactionsTable);

            });
        }
        void TableForMostPurchased(IContainer Container)
        {
            Container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(290);
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    //Add-Ons heading
                    header.Cell().Element(HeadingForTopAddOnsItems);

                    //Coffee heading
                    header.Cell().Element(HeadingForTopCoffees);
                });

                //Add-Ons Table
                table.Cell().Element(PurchasedAddOnsItemTable);

                //Coffee Table
                table.Cell().Element(MostPurchasedCoffesTable);
            });
        }

        //header for sales transaction table
        void SalesTransactionsHeader(IContainer container)
        {
            var pdftitleStyle = TextStyle.Default.FontSize(16).SemiBold();

            string pdftitleName = $"{ReportObj.ReportType} Sales Transaction Report - ({ReportObj.ReportDate})";

            container.Row(row =>
            {

                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"{pdftitleName}").Style(pdftitleStyle);

                    column.Item().PaddingTop(2).Text(text =>
                    {
                        text.Span("Total Revenue: ").FontSize(14);
                        text.Span($"Rs. {ReportObj.TotalRevenue}").FontSize(14);
                    });
                });
            });

        }

        //for sales transaction table
        void SalesTransactionsTable(IContainer container)
        {
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {

                    columns.ConstantColumn(20);
                    columns.ConstantColumn(130);
                    columns.ConstantColumn(90);
                    columns.ConstantColumn(80);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();

                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Customer Name");
                    header.Cell().Element(CellStyle).Text("Phone Number");
                    header.Cell().Element(CellStyle).Text("Employee");
                    header.Cell().Element(CellStyle).Text("Total Amount");
                    header.Cell().Element(CellStyle).Text("Discount Amount");
                    header.Cell().Element(CellStyle).Text("Grand Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // step 3
                foreach (var order in ReportObj.Orders)
                {
                    table.Cell().Element(CellStyle).Text((ReportObj.Orders.IndexOf(order) + 1).ToString());
                    table.Cell().Element(CellStyle).Text(order.CustomerName);
                    table.Cell().Element(CellStyle).Text(order.CustomerPhoneNumber);
                    table.Cell().Element(CellStyle).Text(order.EmployeeUserName);
                    table.Cell().Element(CellStyle).Text($"Rs.{order.OrderTotalAmount}");
                    table.Cell().Element(CellStyle).Text($"Rs.{order.DiscountAmount}");
                    table.Cell().Element(CellStyle).Text($"Rs.{order.OrderTotalAmount - order.DiscountAmount}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }
        void HeadingForTopAddOnsItems(IContainer container)
        {
            var pdftitleStyle = TextStyle.Default.FontSize(12).SemiBold();

            string pdftitleName = "Most Purchased Add-On Items";

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"{pdftitleName}").Style(pdftitleStyle);

                });
            });
        }
        //composes the header for top5 most purhcasesd coffee
        void HeadingForTopCoffees(IContainer container)
        {
            var pdftitleStyle = TextStyle.Default.FontSize(12).SemiBold();

            string pdftitle = "Most Purchased Coffee";

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"{pdftitle}").Style(pdftitleStyle);

                });
            });
        }

        // generates the Top 5 Most Purchased Coffees table.
        void MostPurchasedCoffesTable(IContainer container)
        {
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {

                    columns.ConstantColumn(20);
                    columns.ConstantColumn(150);
                    columns.ConstantColumn(70);

                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Coffee Type");
                    header.Cell().Element(CellStyle).Text("Quantity");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // step 3
                foreach (var coffee in ReportObj.CoffeeList)
                {
                    table.Cell().Element(CellStyle).Text((ReportObj.CoffeeList.IndexOf(coffee) + 1).ToString());
                    table.Cell().Element(CellStyle).Text(coffee.OrderedItemName);
                    table.Cell().Element(CellStyle).Text(coffee.Quantity.ToString());

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }

        // This method generates the Top 5 Most Added Add-on Items table.
        void PurchasedAddOnsItemTable(IContainer container)
        {
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {

                    columns.ConstantColumn(20);
                    columns.ConstantColumn(150);
                    columns.ConstantColumn(70);

                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Add-On Item Name");
                    header.Cell().Element(CellStyle).Text("Quantity");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // step 3
                foreach (var addOnItem in ReportObj.AddOnsList)
                {
                    table.Cell().Element(CellStyle).Text((ReportObj.AddOnsList.IndexOf(addOnItem) + 1).ToString());
                    table.Cell().Element(CellStyle).Text(addOnItem.OrderedItemName);
                    table.Cell().Element(CellStyle).Text(addOnItem.Quantity.ToString());

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }
        }
}
