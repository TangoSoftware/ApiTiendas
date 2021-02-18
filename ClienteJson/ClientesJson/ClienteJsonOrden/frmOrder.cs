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

            URL.Text = "https://ttiendas.axoft.com/api/v2/Aperture/order";

            //URL.Text = "http://localhost/NexoTiendas/api/v2/Aperture/Order";
            //AccessToken.Text = "044f5ff5-74e6-43bf-9e9b-9788214b08b1_10778";
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
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            CustomerDto customerDto = new CustomerDto();
            customerDto.Apartment = customerApartment.Text;
            customerDto.BusinessAddress = customerBusinessAddress.Text;
            customerDto.BusinessName = customerBusinessName.Text;
            customerDto.City = customerCity.Text;
            customerDto.Comments = PrincipalComment.Text;
            try
            {
                customerDto.CustomerID = Convert.ToInt64(customerCustomerID.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            customerDto.Code = customerCode.Text;
            customerDto.DocumentNumber = customerDocumentNumber.Text;
            customerDto.DocumentType = customerDocumentType.Text;
            customerDto.Email = customerEmail.Text;
            customerDto.FirstName = customerFirstName.Text;
            customerDto.Floor = customerFloor.Text;
            customerDto.HouseNumber = customerHouseNumber.Text;
            customerDto.IVACategoryCode = customerIVACategoryCode.Text;
            customerDto.PayInternalTax = customerPayInternalTax.Checked;
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
                orderDto.PaidTotal = Convert.ToDecimal(principalPaidTotal.Text);
                orderDto.FinancialSurcharge = Convert.ToDecimal(principalFinancialSurcharge.Text);
                orderDto.WarehouseCode = Convert.ToString(principalWarehouseCode.Text);                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            
            orderDto.SellerCode = principalSellerCode.Text;
            orderDto.TransportCode = principalTransportCode.Text;
            orderDto.SaleConditionCode = int.TryParse(principalSaleConditionCode.Text,out int saleConditionCode) ? saleConditionCode : 0;
            orderDto.PriceListNumber = int.TryParse(principalPriceListNumber.Text, out int priceListNumber) ? priceListNumber : 0;
            orderDto.OrderID = principalOrderID.Text;
            orderDto.OrderItems = ListaOrderItems;
            orderDto.OrderNumber = principalOrderNumber.Text;
            orderDto.Payments = ListaPayments;
            orderDto.Comment = PrincipalComment.Text;
            orderDto.IvaIncluded = checkIvaIncluded.Checked;
            orderDto.InternalTaxIncluded = checkInternalTaxIncluded.Checked;
            orderDto.ValidateTotalWithPaidTotal = checkValidateTotalWithPaidTotal.Checked;

            CashPaymentDto cashPayment = new CashPaymentDto();
            if (!string.IsNullOrEmpty(cashPaymentTotal.Text)) 
            {
                try
                {
                    cashPayment.PaymentTotal = Convert.ToDecimal(cashPaymentTotal.Text);
                    cashPayment.PaymentID = Convert.ToInt64(cashPaymentId.Text);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                cashPayment.PaymentMethod = cashPaymentMethod.Text;
            };

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
            shipping.Street = shippingStreet.Text;
            shipping.ShippingCode = shippingShippingCode.Text;

            shipping.ShippingCost = 0;
            if (!String.IsNullOrWhiteSpace(shippingShippingCost.Text))
            {
                shipping.ShippingCost = Convert.ToDecimal(shippingShippingCost.Text);
            }

            shipping.ShippingID = 0;
            if (!String.IsNullOrWhiteSpace(shippingShippingID.Text))
            {
                shipping.ShippingID = Convert.ToInt64(shippingShippingID.Text);
            }
        
            if (checkGenerarShipping.Checked)
            {
                orderDto.Shipping = shipping;
            }

            try
            {
                orderDto.Total = Convert.ToDecimal(principalTotal.Text);
                orderDto.TotalDiscount = Convert.ToDecimal(principalTotalDiscount.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

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
                principalOrderID.Text = orderDto.OrderID;
                principalOrderNumber.Text = orderDto.OrderNumber;                           
                try
                {
                    principalCancelOrder.Text = Convert.ToString(orderDto.CancelOrder);
                    principalDate.Text = Convert.ToString(orderDto.Date);
                    principalPaidTotal.Text = Convert.ToString(orderDto.PaidTotal);
                    principalFinancialSurcharge.Text = Convert.ToString(orderDto.FinancialSurcharge);
                    principalWarehouseCode.Text = orderDto.WarehouseCode;
                    principalSellerCode.Text = orderDto.SellerCode;
                    principalTransportCode.Text = orderDto.TransportCode;
                    principalSaleConditionCode.Text = Convert.ToString(orderDto.SaleConditionCode);
                    principalPriceListNumber.Text = Convert.ToString(orderDto.PriceListNumber);
                    PrincipalComment.Text = orderDto.Comment;
                    customerCustomerID.Text = Convert.ToString(orderDto.Customer.CustomerID);
                    checkIvaIncluded.Checked = orderDto.IvaIncluded;
                    checkInternalTaxIncluded.Checked = orderDto.InternalTaxIncluded;
                    checkValidateTotalWithPaidTotal.Checked = orderDto.ValidateTotalWithPaidTotal;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
                customerApartment.Text = orderDto.Customer.Apartment;
                customerBusinessAddress.Text = orderDto.Customer.BusinessAddress;
                customerBusinessName.Text = orderDto.Customer.BusinessName;
                customerCity.Text = orderDto.Customer.City;
                customerCode.Text = orderDto.Customer.Code;
                customerDocumentNumber.Text = orderDto.Customer.DocumentNumber;
                customerDocumentType.Text = orderDto.Customer.DocumentType;
                customerEmail.Text = orderDto.Customer.Email;
                customerFirstName.Text = orderDto.Customer.FirstName;
                customerFloor.Text = orderDto.Customer.Floor;
                customerHouseNumber.Text = orderDto.Customer.HouseNumber;
                customerIVACategoryCode.Text = orderDto.Customer.IVACategoryCode;
                customerPayInternalTax.Checked = orderDto.Customer.PayInternalTax;
                customerLastName.Text = orderDto.Customer.LastName;
                customerMobilePhoneNumber.Text = orderDto.Customer.MobilePhoneNumber;
                customerPhoneNumber1.Text = orderDto.Customer.PhoneNumber1;
                customerPhoneNumber2.Text = orderDto.Customer.PhoneNumber2;
                customerPostalCode.Text = orderDto.Customer.PostalCode;
                customerProvinceCode.Text = orderDto.Customer.ProvinceCode;
                customerStreet.Text = orderDto.Customer.Street;
                customerUser.Text = orderDto.Customer.User;

                ListaOrderItems = (List<OrderItemDto>)orderDto.OrderItems;
                var bindingListOrderItems = new BindingList<OrderItemDto>(ListaOrderItems);
                var sourceOrderItems = new BindingSource(bindingListOrderItems, null);
                dataGridOrderItems.Columns["WarehouseCode"].Visible = false;
                dataGridOrderItems.DataSource = sourceOrderItems;
               

                ListaPayments = (List<PaymentDto>)orderDto.Payments;

                if (ListaPayments != null)
                {
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
                        shippingStreet.Text = orderDto.Shipping.Street;
                        try
                        {
                            shippingShippingCost.Text = Convert.ToString(orderDto.Shipping.ShippingCost);
                            shippingShippingID.Text = Convert.ToString(orderDto.Shipping.ShippingID);
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                }
                if (orderDto.CashPayment != null)
                {
                    try
                    {
                        cashPaymentId.Text = Convert.ToString(orderDto.CashPayment.PaymentID);
                        cashPaymentTotal.Text = Convert.ToString(orderDto.CashPayment.PaymentTotal);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                    cashPaymentMethod.Text = orderDto.CashPayment.PaymentMethod;
                }

                try
                {
                    principalTotal.Text = Convert.ToString(orderDto.Total);
                    principalTotalDiscount.Text = Convert.ToString(orderDto.TotalDiscount);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }

                tabControl1.SelectedIndex = 0;
                MessageBox.Show("Json ingresado");
            }
            else
            {
                MessageBox.Show("Json inválido");
            }
        }
    }
}
