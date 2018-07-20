using Newtonsoft.Json;
using nexoTiendas.ApertureDto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ClienteJsonOrden
{
    public partial class frmOrder : Form
    {
        public frmOrder()
        {
            InitializeComponent();
        }

        public List<OrderItemDto> ListaOrderItems;
        public List<PaymentDto> ListaPayments;

        private void handle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListaOrderItems = new List<OrderItemDto>();
            var bindingListOrderItems = new BindingList<OrderItemDto>(ListaOrderItems);
            var sourceOrderItems = new BindingSource(bindingListOrderItems, null);
            dataGridOrderItems.DataSource = sourceOrderItems;
            dataGridOrderItems.DataError += new DataGridViewDataErrorEventHandler(handle_DataError);

            ListaPayments = new List<PaymentDto>();
            var bindingListPayments = new BindingList<PaymentDto>(ListaPayments);
            var sourcePayments = new BindingSource(bindingListPayments, null);
            dataGridPayments.DataSource = sourcePayments;
            dataGridPayments.DataError += new DataGridViewDataErrorEventHandler(handle_DataError);
        }

        /// <summary>
        /// Genera el json en la solapa resultado a partir de los datos ingresados en el resto de las solapas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerarJson_Click(object sender, EventArgs e)
        {
            OrderDto orderDto = new OrderDto();

            try
            {
                orderDto.CancelOrder = Convert.ToBoolean(principalCancelOrder.Text.ToLower());
            }
            catch { }

            CustomerDto customerDto = new CustomerDto();
            customerDto.Apartment = customerApartment.Text;
            customerDto.BusinessAddress = customerBusinessAddress.Text;
            customerDto.BusinessName = customerBusinessName.Text;
            customerDto.City = customerCity.Text;
            customerDto.Comments = customerComments.Text;
            try
            {
                customerDto.CustomerID = Convert.ToInt64(customerCustomerID.Text);
            }
            catch { }
            customerDto.DocumentNumber = customerDocumentNumber.Text;
            customerDto.DocumentType = customerDocumentType.Text;
            customerDto.Email = customerEmail.Text;
            customerDto.FirstName = customerFirstName.Text;
            customerDto.Floor = customerFloor.Text;
            customerDto.HouseNumber = customerHouseNumber.Text;
            customerDto.IVACategoryCode = customerIVACategoryCode.Text;
            customerDto.LastName = customerLastName.Text;
            customerDto.MobilePhoneNumber = customerMobilePhoneNumber.Text;
            customerDto.PhoneNumber1 = customerPhoneNumber1.Text;
            customerDto.PhoneNumber2 = customerPhoneNumber2.Text;
            customerDto.PostalCode = customerPostalCode.Text;
            customerDto.ProvinceCode = customerProvinceCode.Text;
            customerDto.Street = customerStreet.Text;
            customerDto.User = customerUser.Text;

            orderDto.Customer = customerDto;
            try
            {
                orderDto.Date = Convert.ToDateTime(principalDate.Text);
            }
            catch { }
            orderDto.OrderID = principalOrderID.Text;

            orderDto.OrderItems = ListaOrderItems;

            orderDto.OrderNumber = principalOrderNumber.Text;
            try
            {
                orderDto.PaidTotal = Convert.ToDecimal(principalPaidTotal.Text);
            }
            catch { }
            try
            {
                orderDto.FinancialSurcharge = Convert.ToDecimal(principalFinancialSurcharge.Text);
            }
            catch { }

            orderDto.Payments = ListaPayments;

            CashPaymentDto cashPayment = new CashPaymentDto();
            try
            {
                cashPayment.PaymentTotal = Convert.ToDecimal(cashPaymentTotal.Text);
            }
            catch { }
            try
            {
                cashPayment.PaymentID = Convert.ToInt64(cashPaymentId.Text);
            }
            catch { }
            cashPayment.PaymentMethod = cashPaymentMethod.Text;

            if (checkGenerarCashPayment.Checked)
            {
                orderDto.CashPayment = cashPayment;
            }

            ShippingDto shipping = new ShippingDto();
            shipping.Apartment = shippingApartment.Text;
            shipping.City = shippingCity.Text;
            shipping.DeliversMonday = shippingDeliversMonday.Text;
            shipping.DeliversTuesday = shippingDeliversTuesday.Text;
            shipping.DeliversWednesday = shippingDeliversWednesday.Text;
            shipping.DeliversThursday = shippingDeliversThursday.Text;
            shipping.DeliversFriday = shippingDeliversFriday.Text;
            shipping.DeliversSaturday = shippingDeliversSaturday.Text;
            shipping.DeliversSunday = shippingDeliversSunday.Text;
            shipping.DeliveryHours = shippingDeliveryHours.Text;
            shipping.Floor = shippingFloor.Text;
            shipping.HouseNumber = shippingHouseNumber.Text;
            shipping.PhoneNumber1 = shippingPhoneNumber1.Text;
            shipping.PhoneNumber2 = shippingPhoneNumber2.Text;
            shipping.PostalCode = shippingPostalCode.Text;
            shipping.ProvinceCode = shippingProvinceCode.Text;
            try
            {
                shipping.ShippingCost = Convert.ToDecimal(shippingShippingCost.Text);
            }
            catch { }
            try
            {
                shipping.ShippingID = Convert.ToInt64(shippingShippingID.Text);
            }
            catch { }
            shipping.Street = shippingStreet.Text;

            if (checkGenerarShipping.Checked)
            {
                orderDto.Shipping = shipping;
            }

            try
            {
                orderDto.Total = Convert.ToDecimal(principalTotal.Text);
            }
            catch { }
            try
            {
                orderDto.TotalDiscount = Convert.ToDecimal(principalTotalDiscount.Text);
            }
            catch { }

            string json = JsonConvert.SerializeObject(orderDto, Formatting.Indented);
            resultadoSerializado.Text = json;
            tabControl1.SelectTab("tabResultado");
        }

        /// <summary>
        /// Envía el json de la solapa resultado a la URL y con el access token ingresados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enviar_Click(object sender, EventArgs e)
        {
            try
            {
                OrderDto testOrder = JsonConvert.DeserializeObject<OrderDto>(resultadoSerializado.Text);
                bool enviarJson = true;
                if (testOrder == null)
                {
                    var confirmResult = MessageBox.Show("El json no es un objeto orden válido, desea enviarlo de todas formas?",
                                     "Json no válido",
                                     MessageBoxButtons.YesNo);
                    if (confirmResult != DialogResult.Yes)
                    {
                        enviarJson = false;
                    }
                }
                if (enviarJson)
                {
                    string result = "";
                    using (var client = new WebClient())
                    {
                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("AccessToken", AccessToken.Text);
                        client.Encoding = Encoding.UTF8;
                        result = client.UploadString(URL.Text, "POST", resultadoSerializado.Text);
                    }
                    MessageBox.Show(result);
                }
            }
            catch (WebException Wex)
            {
                string resp = new StreamReader(Wex.Response.GetResponseStream()).ReadToEnd();
                
                MessageBox.Show(resp);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        /// <summary>
        /// Toma el json de la solapa resultado y lo asigna a los distintos campos correspondientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CargarJson_Click(object sender, EventArgs e)
        {
            OrderDto orderDto = JsonConvert.DeserializeObject<OrderDto>(resultadoSerializado.Text);

            if (orderDto != null)
            {
                try
                {
                    principalCancelOrder.Text = Convert.ToString(orderDto.CancelOrder);
                }
                catch { }

                customerApartment.Text = orderDto.Customer.Apartment;
                customerBusinessAddress.Text = orderDto.Customer.BusinessAddress;
                customerBusinessName.Text = orderDto.Customer.BusinessName;
                customerCity.Text = orderDto.Customer.City;
                customerComments.Text = orderDto.Customer.Comments;
                try
                {
                    customerCustomerID.Text = Convert.ToString(orderDto.Customer.CustomerID);
                }
                catch { }
                customerDocumentNumber.Text = orderDto.Customer.DocumentNumber;
                customerDocumentType.Text = orderDto.Customer.DocumentType;
                customerEmail.Text = orderDto.Customer.Email;
                customerFirstName.Text = orderDto.Customer.FirstName;
                customerFloor.Text = orderDto.Customer.Floor;
                customerHouseNumber.Text = orderDto.Customer.HouseNumber;
                customerIVACategoryCode.Text = orderDto.Customer.IVACategoryCode;
                customerLastName.Text = orderDto.Customer.LastName;
                customerMobilePhoneNumber.Text = orderDto.Customer.MobilePhoneNumber;
                customerPhoneNumber1.Text = orderDto.Customer.PhoneNumber1;
                customerPhoneNumber2.Text = orderDto.Customer.PhoneNumber2;
                customerPostalCode.Text = orderDto.Customer.PostalCode;
                customerProvinceCode.Text = orderDto.Customer.ProvinceCode;
                customerStreet.Text = orderDto.Customer.Street;
                customerUser.Text = orderDto.Customer.User;

                try
                {
                    principalDate.Text = Convert.ToString(orderDto.Date);
                }
                catch { }
                principalOrderID.Text = orderDto.OrderID;

                ListaOrderItems = (List<OrderItemDto>)orderDto.OrderItems;
                var bindingListOrderItems = new BindingList<OrderItemDto>(ListaOrderItems);
                var sourceOrderItems = new BindingSource(bindingListOrderItems, null);
                dataGridOrderItems.DataSource = sourceOrderItems;

                principalOrderNumber.Text = orderDto.OrderNumber;
                try
                {
                    principalPaidTotal.Text = Convert.ToString(orderDto.PaidTotal);
                }
                catch { }

                try
                {
                    principalFinancialSurcharge.Text = Convert.ToString(orderDto.FinancialSurcharge);
                }
                catch { }

                ListaPayments = (List<PaymentDto>)orderDto.Payments;
                var bindingListPayments = new BindingList<PaymentDto>(ListaPayments);
                var sourcePayments = new BindingSource(bindingListPayments, null);
                dataGridPayments.DataSource = sourcePayments;

                if (orderDto.Shipping != null)
                {
                    shippingApartment.Text = orderDto.Shipping.Apartment;
                    shippingCity.Text = orderDto.Shipping.City;
                    shippingDeliversMonday.Text = orderDto.Shipping.DeliversMonday;
                    shippingDeliversTuesday.Text = orderDto.Shipping.DeliversTuesday;
                    shippingDeliversWednesday.Text = orderDto.Shipping.DeliversWednesday;
                    shippingDeliversThursday.Text = orderDto.Shipping.DeliversThursday;
                    shippingDeliversFriday.Text = orderDto.Shipping.DeliversFriday;
                    shippingDeliversSaturday.Text = orderDto.Shipping.DeliversSaturday;
                    shippingDeliversSunday.Text = orderDto.Shipping.DeliversSunday;
                    shippingDeliveryHours.Text = orderDto.Shipping.DeliveryHours;
                    shippingFloor.Text = orderDto.Shipping.Floor;
                    shippingHouseNumber.Text = orderDto.Shipping.HouseNumber;
                    shippingPhoneNumber1.Text = orderDto.Shipping.PhoneNumber1;
                    shippingPhoneNumber2.Text = orderDto.Shipping.PhoneNumber2;
                    shippingPostalCode.Text = orderDto.Shipping.PostalCode;
                    shippingProvinceCode.Text = orderDto.Shipping.ProvinceCode;
                    try
                    {
                        shippingShippingCost.Text = Convert.ToString(orderDto.Shipping.ShippingCost);
                    }
                    catch { }
                    try
                    {
                        shippingShippingID.Text = Convert.ToString(orderDto.Shipping.ShippingID);
                    }
                    catch { }
                    shippingStreet.Text = orderDto.Shipping.Street;
                }

                if (orderDto.CashPayment != null)
                {
                    try
                    {
                        cashPaymentId.Text = Convert.ToString(orderDto.CashPayment.PaymentID);
                    }
                    catch { }
                    try
                    {
                        cashPaymentTotal.Text = Convert.ToString(orderDto.CashPayment.PaymentTotal);
                    }
                    catch { }
                    cashPaymentMethod.Text = orderDto.CashPayment.PaymentMethod;
                }

                try
                {
                    principalTotal.Text = Convert.ToString(orderDto.Total);
                }
                catch { }
                try
                {
                    principalTotalDiscount.Text = Convert.ToString(orderDto.TotalDiscount);
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Json inválido");
            }
        }
    }
}
