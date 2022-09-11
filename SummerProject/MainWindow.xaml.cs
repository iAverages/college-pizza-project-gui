using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SummerProject
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class OrderItem
        {
            public string Name { get; set; }
            public Double Price { get; set; }

            public OrderItem(string name, double price)
            {
                Name = name;
                Price = price;
            }
        }

        private Boolean hasItems = false;
        private Double orderPrice = 0;
        private OrderItem[] items;

        public MainWindow()
        {
            InitializeComponent();
            this.hasItems = false;
            this.items = Array.Empty<OrderItem>();
        }

        private void NewOrder(object sender, RoutedEventArgs e)
        {
            this.hasItems = false;
            TextBlock orderText = new TextBlock();
            orderText.Text = "No items added";
            items = Array.Empty<OrderItem>();
            this.orderPrice = 0;
            orderTotal.Text = "Total: ";
            orders.Children.Clear();
            orders.Children.Add(orderText);
        }

        private void AddPizza(OrderItem order)
        {
            if (!this.hasItems)
            {
                this.hasItems = true;
                // Remove placholder text
                orders.Children.Clear();
            }

            // Auto generate grid col and row
            Boolean isEven = items.Length % 2 == 0;
            int col = isEven ? 0 : 1;
            int row = isEven ? items.Length / 2 : (items.Length - 1) / 2;

            TextBlock orderText = new TextBlock();
            orderText.Text = order.Name + " - £" + order.Price;
            Grid.SetColumn(orderText, col);
            Grid.SetRow(orderText, row);

            orders.Children.Add(orderText);
            OrderItem[] newOrder = { order };
            items = items.Concat(newOrder).ToArray();
            this.AddPrice(order.Price);
        }

        private void AddPrice(Double addPrice)
        {
            this.orderPrice += addPrice;
            string feesText = "No fees or discounts added";
            Double fees = 0;
            if (this.orderPrice > 20)
            {
                feesText = "20% Discount applied";
                // 20% Discount
                fees = -Math.Round(this.orderPrice, 2) * 0.2;
            }
            else if (this.orderPrice < 9.50)
            {
                feesText = "£1.50 devliery fees applied";
                fees = 1.50;
            }

            this.orderPrice += fees;
            orderTotal.Text = "Total: £" + Math.Round(this.orderPrice, 2);
            feesBlock.Text = feesText;

        }

        private static Boolean IsValidPhoneNumber(string email)
        {
            try
            {
                // Basic UK phone number validation
                return Regex.IsMatch(email, @"\d{11}");
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private async void SaveOrder(object sender, RoutedEventArgs e)
        {
            TextBox[] fields = { firstName, lastName, address, phoneNumber };
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Text == "")
                {
                    MessageBox.Show("Failed validation for " + fields[i].Name);
                    return;
                }
            }

            if (!IsValidPhoneNumber(phoneNumber.Text))
            {
                MessageBox.Show("Failed validation for " + phoneNumber.Name);
                return;
            }
            MessageBox.Show("Order saved!");
            string saveText = $"\n{firstName.Text},{lastName.Text},{address.Text},{phoneNumber.Text},{this.orderPrice}";
            foreach (OrderItem item in items)
            {
                saveText += $"{item.Name}|{item.Price},";
            }
            await File.AppendAllTextAsync("orders.csv", saveText);
        }

        private void AddPiri(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("Piri", 8.80));
        }

        private void AddHawaii(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("Hawaii", 8.90));
        }

        private void AddBBQ(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("BBQ", 7.90));
        }

        private void AddMeat(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("Meat", 8.10));
        }

        private void AddTheWorks(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("TheWorks", 9.90));
        }

        private void AddMexican(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("Mexican", 9.70));
        }

        private void AddMediteranian(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("Mediteranian", 9.50));
        }

        private void AddCheese(object sender, RoutedEventArgs e)
        {
            this.AddPizza(new OrderItem("Cheese", 7.50));
        }
    }
}
