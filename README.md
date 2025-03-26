<a name="inicio"></a>
Tango Software - API REST de Tango Tiendas
=======

- [Instalación](#instalacion)
  - [Versiones soportadas de Tango Tiendas](#versiones)
  - [Generalidades](#generalidades)
  - [Ambientes](#ambientes)
  - [Asociar aplicación con API](#asociarapi)
- [Recepción de órdenes API](#ordenes)
  - [Novedades](#novedades)
  - [Datos del JSON](#djson)
  - [Ordenes por Lote](#lote)
  - [Tablas de Referencia](#tablas)
  - [Ejemplo de JSON de una órden](#ejemplojson)
- [Consulta de datos](#subida)
  - [Configuración](#configuracion)
  - [Conceptos básicos](#ConceptosBasicos)
  - [Recursos de consulta](#recursos)

<a name="instalacion"></a>

## Instalación

<a name="versiones"></a>

### Versiones soportadas de Tango Tiendas

##### Recepción de órdenes por API

Para implementar la API de Tango Tiendas, debe tener instalada la versión vigente  o la inmediata anterior del sistema Tango Gestión ó Tango Punto de Venta Argentina. Comuníquese con su distribuidor para mayor información. Además, es necesario contar con el módulo de tesorería y la licencia **Tango Tiendas Full**  activada.

##### Consulta de datos

Los datos comienzan a estar disponibles cuando se cumplen las siguientes condiciones:  

* La versión del sistema Tango instalado es la vigente  o la inmediata anterior.
* La licencia de **Tango Gestión ó Tango Punto de Venta Argentina** tiene la licencia **Tango Tiendas Full** activada.
* Se ejecuto el wizard de la aplicación Nexo Tiendas en su sistema **Tango Gestión ó Tango Punto de Venta Argentina** para asociar la empresa que desea utilizar.
* Se accedió a Tango Tiendas / API y se obtuvo el AccessToken de su cuenta.

Importante: El requerimiento minimo de la API de Tiendas de TLS corresponde a la versión 1.2. Tango Tiendas no da soporte a TLS 1.0 ni TLS 1.1.

<a name="generalidades"></a>

#### Generalidades

[<sub>Volver</sub>](#inicio)

Esta versión soporta órdenes de pedido únicamente en moneda nacional argentina.

Aceptando hasta 2 decimales en los datos de importes y precios.

<a name="ambientes"></a>

#### Ambientes

[<sub>Volver</sub>](#inicio)

• Ambiente de testeo

Para configurar un ambiente de testeo puede asociar una empresa de nube con una empresa ejemplo de Tango Gestión o Tango Punto de Venta.

• Ambiente de producción

Para configurar el ambiente de producción debe asociar una empresa de nube con una empresa operativa de Tango Gestión o Tango Punto de Venta.

<a name="asociarapi"></a>

### Asociar aplicación con API

[<sub>Volver</sub>](#inicio)

Luego de haber vinculado una empresa de nube con una empresa de Tango Gestión o Tango Punto de Venta, acceda a Tango Tiendas / API para obtener el un Access Token que le permitirá enviar órdenes de pedido a Tango.

A continuación, explicamos la configuración a aplicar desde Tango Tiendas, para cargar órdenes de pedido a través de una interfaz API.

Presione el botón &quot;Obtener&quot; e introduzca un nombre de referencia para la **API**.

Al presionar el botón &quot;Aceptar&quot; se generará un Access token con el cual se podrá conectar la **API** con **Tango Tiendas**.

![imagenapi](https://github.com/TangoSoftware/ApiTiendas/blob/master/api.jpg)

A partir de ese momento ya puede comenzar a utilizar la **API** en **Tango Tiendas** y manejar sus ventas desde **Tango Gestión o Tango Punto de Venta**.

El Access token obtenido se debe utilizar en el header de la llamada en la key "accesstoken".

**La URL del servicio de API para verificación es (POST):**

[https://tiendas.axoft.com/api/Aperture/dummy](https://tiendas.axoft.com/api/Aperture/dummy)

**Formato de respuestas del metodo POST Dummy:**

En caso de que el acceso sea válido:

```
{"Status":0,"Message":"Valid AccessToken","Data":null,"isOk":true}
```

En caso de que el acceso sea inválido:

```
{"Status":1,"Message":"Invalid AccessToken ","Data":null,"isOk":false}
```

<a name="ordenes"></a>

## Recepción de órdenes API

[<sub>Volver</sub>](#inicio)

**La URL del servicio de API para órdenes es:**

[https://tiendas.axoft.com/api/Aperture/order](https://tiendas.axoft.com/api/Aperture/order)

Tenga en cuenta los siguientes temas:

- [Notificaciones](#notificaciones)

- [Preguntas frecuentes](#faqs)

- [Novedades en el JSON de la orden](#novedades)

<a name="notificaciones"></a>

### Notificaciones

Si desea recibir notificaciones, en la configuración de la **API** debe marcar el check y configurar una URL donde recibirá las notificaciones. Dicha URL debe corresponder a un recurso POST de una web API cliente que actúe como webhook de notificaciones.

**Importante**: el servidor del webhook de notificaciones debe tener soporte de TLS 1.2. Tango Tiendas no da soporte a TLS 1.0 ni TLS 1.1.

Se enviarán notificaciones a la URL configurada de los siguientes eventos:

• Al intentar generar el pedido de una orden de Tango Tiendas y la misma sea observada. Por ejemplo que se informe una lista de precios inexistente. (Se enviará el Tópico: OrderObserved)

• Al generar el pedido de una orden de Tango Tiendas. (Se enviará el Tópico: OrderProcessed)

• Al rechazar una orden de pedido. (Se enviará el Tópico: OrderRejected)

• Al facturar el pedido generado. (Se enviará el Tópico: OrderBilled)

• Al disponer de un comprobante electrónico de facturación en PDF. (Se enviará el Tópico: InvoiceFile)

• Al actualizar el precio de un artículo. (Se enviará el Tópico: PriceProductUpdate)

• Al actualizar el saldo de stock de un artículo. (Se enviará el Tópico: StockProductUpdate)

**Formato de JSON de notificación:**

```
{
  "Topic": "OrderObserved",
  
  "Resource": "1",
  
  "Message": "Lista de precios inexistente"
}

{

  "Topic": "OrderProcessed",

  "Resource": "1",
  
  "Message": ""
}

```

**Importante**: los tópicos del JSON son case sensitive, de forma que deben respetarse las mayúsculas iniciales de "Topic" y "Resource".

**Aclaración**: 

• La propiedad "Resource" corresponde al identificador de la orden informado en el JSON (OrderId).

• Para el caso que se produzca una actualización del stock de un artículo, al notificar el tópico StockProductUpdate la propiedad "Resource" corresponde a la identificación (Id) del registro modificado.

• Para el caso que se produzca una actualización del precio de un artículo, al notificar el tópico PriceProductUpdate la propiedad "Resource" corresponde a la identificación (Id) del registro modificado.

• Para el caso de los artículos parametrizados para que generan movimientos de stock, al notificar el tópico OrderProcessed corresponden a cantidades comprometidas.

• Para el caso de los artículos parametrizados para que generan movimientos de stock, al notificar el tópico OrderBilled corresponden a cantidades de stock.

<a name="faqs"></a>

### Preguntas Frecuentes

- **¿Cómo debo armar el JSON para cargar una orden a través de la API?**

En la solapa API en Tango Tiendas se muestra un Ejemplo del JSON. Además, se puede ver un Modelo, Respuesta, Notificación y Respuesta notificación.Datos del JSON.

- **¿El Access token se genera una sola vez?**

Se genera un Access token por cuenta. Si se elimina la cuenta, al crear una nueva se generará un nuevo Access token.

- **¿Qué pasa si elimino el Access token?**

Perderá el acceso para enviar órdenes de pedidos desde su API a Tango y estas tampoco serán recibidas en Revisión de pedidos de Tango Tiendas.

- **¿A nombre de quién se emite la factura de venta?**

Cuando en la orden de pedido viene informado el número del C.U.I.L / C.U.I.T. ó D.N.I. y se corresponden con datos de A.F.I.P., será considerada esta información para emitir la factura de ventas en la ausencia de esta información se tomará el Nombre Comercial indicado. Cuando no se informa el número del C.U.I.L / C.U.I.T. ó D.N.I. se utilizará el nombre y apellido ingresado en la orden de pedido.

<a name="novedades"></a>

### Novedades en el JSON de la orden

#### Consideraciones al enviar órdenes para Doble Unidad de Medida

Las características que posee un artículo con doble unidad de medida son las siguientes:

- **Stock**

• Código de UM de stock 1 (Precios y costos): indica la unidad de medida del artículo. Tenga en cuenta que esta unidad es la que se utiliza para expresar los precios del artículo, calcular los costos y expresar los saldos.

• Código de UM de stock 2: indica la segunda unidad de medida del artículo. Es otra unidad de stock en la que se expresa el saldo.

• UM de control de stock: determina entre la unidad de stock 1 (unidad de precios y costos) o la de stock 2 cual realiza el control de stock.
La unidad de medida definida en este parámetro, es la que se tomará para controlar la disponibilidad del stock al momento de realizar una descarga de stock, de igual manera es la unidad que usará el sistema para comprometer el stock.

• Equivalencia: indica la equivalencia que existe entre la unidad de stock 2 respecto a la unidad de stock 1.

```
Ejemplo…

Se desea vender hormas de queso, tenemos la siguiente configuración:

Unidad de stock 1 = Kilos
Unidad de stock 2 = Horma
Equivalencia = 2 Kilos (Una horma equivale a 2 kilos)
```

- **Ventas**

• Código de presentación de ventas: indica a cuantas unidades de stock equivale una unidad de ventas.
Si el artículo lleva doble unidad de medida, la equivalencia de ventas es hacia la unidad de medida de stock 2, caso contrario la equivalencia de ventas es hacia la unidad de stock 1.

• Equivalencia: indica la equivalencia con la unidad de medida de stock seleccionada.


- **Unidad de Medida Seleccionada (SelectMeasureUnit)**

Según la parametrización que posea el artículo (Simple o Doble Unidad de Medida) se podrá indicar los siguientes valores:

• V: Ventas

• P: Stock 1

• S: Stock 2

**Nota**: Para el caso de un artículo simple se podrá indicar con P (Stock 1) la unidad elegida al momento de generar el pedido. 


- **Ejemplos**

```
{
  "Date": "2022-02-10T00:00:00",
  "Total": 30.0,
  "PaidTotal": 30.0,
  "FinancialSurcharge": 0.0,
  "WarehouseCode": "1",
  "SellerCode": "1",
  "TransportCode": "01",
  "SaleConditionCode": "1",
  "OrderID": "1000",    
  "OrderNumber": "1000",
  "OrderCounterfoil": 10, // Informa el número de Talonario de Pedidos
  "ValidateTotalWithPaidTotal": false,
  "ValidateTotalWithItems": false,  //Para el caso de DUM donde se informe unidad de Ventas y la equivalencia sea distinto de 1 
  "Customer": {
    "CustomerID": 1000,
    "Code": "",      
    "DocumentType": "96",
    "DocumentNumber": "99999999",
    "IVACategoryCode": "CF",
    "User": "Test",
    "Email": "test@axoft.com",
    "FirstName": "Test",
    "LastName": "Test",
    "BusinessName": "",        
    "Street": "Cerrito",
    "HouseNumber": "1000",
    "Floor": "",
    "Apartment": "",
    "City": "CABA",
    "ProvinceCode": "01",
    "PostalCode": "1000",
    "PhoneNumber1": "9999-9999",
    "PhoneNumber2": "99-9999-9999",
    "BusinessAddress": "Dirección negocio",
    "NumberListPrice": 10
  },
  "OrderItems": [
    {
      "ProductCode": "1000",
      "SKUCode": "ART_DOBLEUNIDAD",    
      "VariantCode": null,        
      "Description": "Artículo de doble unidad de medida",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 30.0,  
      "DiscountPercentage": 0.0,
      "MeasureCode":"UNI",  //Código de medida con el cual se generará el pedido
      "SelectMeasureUnit": "V" //Unidad de medida seleccionada (P: Stock 1 - Precios y Costos;  S: Stock 2 ;  V: Ventas) con la cual se generará el pedido
    }
  ],
 "CashPayment": {
    "PaymentID": 1000,
    "PaymentMethod": "MPA",
    "PaymentTotal": 30.0
  }
}
```
Para este caso se utiliza un artículo con doble unidad de medida cuya característa en Tango es:

• Unidad de stock 1 = KILOGRAMOS (Kilogramos)

• Unidad de stock 2 = UNI (Unidades)

• Equivalencia = 3 Kilos (Una unidad equivale a 3 kilos)


Y se informa en el JSON de la orden lo siguiente:

• SelectMeasureUnit (Unidad de medida seleccionada): V (Ventas) 

• MeasureCode (Código de medida): UNI (Unidades)

• UnitPrice: 30 (El precio informado es el correspondiente a la venta, ya que al momento de generar el pedido se expresará en unidad de medida de Stock1) 

```
{
  "Date": "2022-02-10T00:00:00",
  "Total": 10.0,
  "PaidTotal": 10.0,
  "FinancialSurcharge": 0.0,
  "WarehouseCode": "1",
  "SellerCode": "1",
  "TransportCode": "01",
  "SaleConditionCode": "1",
  "OrderID": "1000",    
  "OrderNumber": "1000",
  "OrderCounterfoil": 10, // Informa el número de Talonario de Pedidos
  "ValidateTotalWithPaidTotal": false,
  "Customer": {
    "CustomerID": 1000,
    "Code": "",      
    "DocumentType": "96",
    "DocumentNumber": "99999999",
    "IVACategoryCode": "CF",
    "User": "Test",
    "Email": "test@axoft.com",
    "FirstName": "Test",
    "LastName": "Test",
    "BusinessName": "",        
    "Street": "Cerrito",
    "HouseNumber": "1000",
    "Floor": "",
    "Apartment": "",
    "City": "CABA",
    "ProvinceCode": "01",
    "PostalCode": "1000",
    "PhoneNumber1": "9999-9999",
    "PhoneNumber2": "99-9999-9999",
    "BusinessAddress": "Dirección negocio",
    "NumberListPrice": 10
  },
  "OrderItems": [
    {
      "ProductCode": "1000",
      "SKUCode": "ART_DOBLEUNIDAD",    
      "VariantCode": null,        
      "Description": "Artículo de doble unidad de medida",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 10.0,
      "DiscountPercentage": 0.0,
      "MeasureCode":"KILOGRAMOS",  //Código de medida con el cual se generará el pedido
      "SelectMeasureUnit": "P" //Unidad de medida seleccionada (P: Stock 1 - Precios y Costos;  S: Stock 2 ;  V: Ventas) con la cual se generará el pedido
    }
  ],
 "CashPayment": {
    "PaymentID": 1000,
    "PaymentMethod": "MPA",
    "PaymentTotal": 10.0
  }
}
```

Para este caso se utiliza un artículo con doble unidad de medida cuya característa en Tango es:

• Unidad de stock 1 = KILOGRAMOS (Kilogramos)

• Unidad de stock 2 = UNI (Unidades)

• Equivalencia = 3 Kilos (Una unidad equivale a 3 kilos)


Y se informa en el JSON de la orden lo siguiente:

• SelectMeasureUnit (Unidad de medida seleccionada): P (Stock1) 

• MeasureCode (Código de medida): KILOGRAMOS (Kilogramos)

• UnitPrice: 10 (El precio informado es el correspondiente a la unidad de medida de Stock1) 

#### Consideraciones al enviar órdenes

- **Condición de venta**

Si la condición de venta es distinto de 'Contado', es posible que al valor de la factura se le apliquen cargos propios de dicha condición (Ej. 30/60/90 días con un 2% de interes).

- **Transporte**

Si la "Condicíón de Venta" es 'Contado' (o en su defecto no se informa), entonces se válida que el código de tranporte informado no tenga recargo (SurchargePercentage = 0).

- **Pagos**

Si la "Condición de Venta" es distinto de 'Contado', entonces se válida que no se informen los tópicos de:

• CashPayments.

• Payments

- **General**

Si ninguno de estos códigos se informan, se mantiene el comportamiento actual.

<a name="djson"></a>

### Datos del JSON

[<sub>Volver</sub>](#inicio)

A continuación, se detalla a modo orientativo, el contenido de cada uno de los datos del JSON

<a name="topicoprincipal"></a>
**Tópico principal**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.

| **Campo**                      | **Requerido**                                       | **Descripción**                                                                                                     | **Tipo de Dato**                                                                                       | **Valores Posibles / Ejemplos**                                                                                                                                          |
| ------------------------------ | --------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **OrderID**                    | Si                                                  | Identificador de la orden. Debe ser distinto para cada operación.                                                   | Alfanumérico de hasta 200 caracteres                                                                   | &gt;0                                                                                                                                                                    |
| **OrderNumber**                | Si                                                  | Número de la orden. Es el número con el cual podrá identificar la orden desde revisión de pedidos de Tango Tiendas  | Alfanumérico de hasta 200 caracteres                                                                   |                                                                                                                                                                          |
| **Date**                       | Si                                                  | Fecha de la orden. Puede ser anterior a 30 días de la fecha actual.                                                  | Datetime                                                                                               | yyyy-MM-ddTHH:mm:ss                                                                                                                                                      |
| **Total**                      | Si                                                  | Es el importe total de la orden. Sólo válido en pesos argentinos.                                                   | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales. En el caso de que el valor Total ingresado corresponda a un número con más de dos decimales, se deberá redondear al número más cercano. El método de redondeo a aplicar será hacia arriba, si el decimal siguiente >= 5, y hacia abajo si < 5. | &gt;=0 ∑[(OrderItems.Quantity x OrderItems.UnitPrice) – OrderItems.DiscountPorcentage)] + Shipping.ShippingCost + Principal.FinancialSurcharge – Principal.TotalDiscount |
| **TotalDiscount**              | No                                                  | Importe de descuento total de la operación. Sólo valido en pesos argentinos.                                        | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;=0&lt; Principal.Total                                                                                                                                               |
| **PaidTotal**                  | Solo si se informa el tópico Payments o CashPayments (en reemplazo de CashPayment) | Importe total pagado. Sólo válido en pesos argentinos.                                                              | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;=0 ∑(Payments.Installments \* Payments.InstallmentsAmount) + ∑(CashPayments.PaymentTotal)                                                                                |
| **FinancialSurcharge**         | No                                                  | Importe del recargo financiero. Sólo válido en pesos argentinos.                                                    | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;= 0                                                                                                                                                                  |
| **WarehouseCode**              | No                                                  | Código del depósito. Si el depósito no existe o está inhabilitado en Tango, no se podrá generar el pedido.          | Alfanumérico de hasta 10 caracteres                                                                     |
| **SellerCode**                 | No                                                  | Código del vendedor. Si el vendedor no existe o está inhabilitado en Tango, no se podrá generar el pedido.          | Alfanumérico de hasta 12 caracteres                                                                    |                                                                                                                                                                          |
| **TransportCode**              | No                                                  | Código del transporte. Si el transporte no existe o está inhabilitado en Tango, no se podrá generar el pedido.      | Alfanumérico de hasta 12 caracteres                                                                    |                                                                                                                                                                          |
| **SaleConditionCode**          | No                                                  | Condición de venta. Si la condición de venta no existe o está inhabilitado en Tango, no se podrá generar el pedido. | Numérico de tipo entero hasta 10 posiciones                                                            |                                                                                                                                                                          |
| **PriceListNumber**            | No                                                  | Número de lista de precios.                                                                                         | Numérico de tipo entero hasta 4 posiciones                                                             |                                                                                                                                                                          |
| **IvaIncluded**                | No (Requerido solo si se informa PriceListNumber)   | Indica que los importes informados incluyen IVA                                                                     | De tipo lógico                                                                                         | True/False                                                                                                                                                               |
| **InternalTaxIncluded**        | No (Requerido solo si se informa PriceListNumber)   | Indica que los importes informados incluyen impuestos internos                                                      | De tipo lógico                                                                                         | True/False                                                                                                                                                               |
| **CancelOrder**                | No                                                  | Indica que la orden está cancelada                                                                                  | De tipo lógico                                                                                         | True/False                                                                                                                                                               |
| **CancelReason**               | No                                                  | Indica el motivo por el cual la orden fue cancelada  | Alfanumérico de hasta 200 carácteres                   | El comprador se arrepintió                                      |
| **CancelDate**                 | Si CancelOrder es True se comporta como un campo requerido | Fecha de cancelación de la orden. No puede ser anterior a la fecha de la orden.                                                  | Datetime                                                                            | yyyy-MM-ddTHH:mm:ss | **ValidateTotalWithPaidTotal** | Si                                                  | Indica si al momento de enviar la orden se valida el total de la orden con el total pagado.                         | De tipo lógico                                                                                         | True/False                                                                                                                                                               |
| **AgreedWithSeller**           | No                                                  | Indica si el pago de la orden se acuerda con el vendedor                                                            | De tipo lógico                                                                                         | True/False                                                                                                                                                               |
| **InvoiceCounterfoil**         | No                                                  | Número de talonario de facturación asociado a la orden                                                                       | Numérico de tipo entero de hasta 4 posiciones                                                          | &gt;= 0 , <= 9999                                                                                                                                                        |
| **Comment**          | No            | Representa los comentarios realizados por el comprador en la orden| Alfanumérico de hasta 280 caracteres        | El pedido será recibido por |
| **OrderCounterfoil**         | No                                                  | Número de talonario de pedido a utilizar                                                                                         | Numérico de tipo entero de hasta 4 posiciones                                                          | &gt;= 0 , <= 9999                                                                                                                                                        |

<a name="topicocustomer"></a>
**Tópico Customer**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.

| **Campo**             | **Requerido** | **Descripción**                                                                                                                             | **Tipo de Dato**                            | **Valores Posibles / Ejemplos**                          |
| --------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------- | -------------------------------------------------------- |
| **CustomerId**        | Si            | Identificador del cliente.                                                                                                                  | Numérico de tipo entero hasta 10 posiciones | &gt;0                                                    |
| **Code**              | No            | Código del cliente. Si el cliente no existe en Tango, no se podrá generar el pedido                                                         | Alfanumérico de hasta 10 caracteres          |                                                          |
| **DocumentType**      | Si            | Código del tipo de documento.                                                                                                               | Numérico con longitud de 2 posiciones       | Ver Tablas de Referencia, [Tipo de Documento](#tipodoc). |
| **DocumentNumber**    | No            | Número de documento sin símbolos ni puntuaciones.                                                                                           | Alfanumérico de hasta 20 caracteres         |                                                          |
| **User**              | Si            | Usuario de la tienda.                                                                                                                       | Alfanumérico de hasta 200 caracteres        |                                                          |
| **BusinessName**      | No            | Razón social del cliente a nombre de quién se emitirá la factura.                                                                           | Alfanumérico de hasta 200 caracteres        |                                                          |
| **FirstName**         | No            | Nombre del cliente. Se utilizará para emitir la factura si mediante el C.U.I.L / C.U.I.T. / D.N.I. no se encontraron datos en la A.F.I.P.   | Alfanumérico de hasta 200 caracteres        |                                                          |
| **LastName**          | No            | Apellido del cliente. Se utilizará para emitir la factura si mediante el C.U.I.L / C.U.I.T. / D.N.I. no se encontraron datos en la A.F.I.P. | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Street**            | No            | Calle del domicilio del cliente.                                                                                                            | Alfanumérico de hasta 200 caracteres        |                                                          |
| **HouseNumber**       | No            | Altura del domicilio del cliente.                                                                                                           | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Floor**             | No            | Piso del domicilio del cliente.                                                                                                             | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Apartment**         | No            | Departamento del domicilio del cliente.                                                                                                     | Alfanumérico de hasta 200 caracteres        |                                                          |
| **City**              | No            | Localidad del domicilio del cliente.                                                                                                        | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Email**             | Si            | Correo electrónico del cliente.                                                                                                             | Alfanumérico de hasta 255 caracteres        | cliente@mail.com                                         |
| **MobilePhoneNumber** | No            | Número de celular del cliente.                                                                                                              | Alfanumérico de hasta 30 caracteres         |                                                          |
| **BusinessAdress**    | No            | Dirección comercial del cliente.                                                                                                            | Alfanumérico de hasta 255 caracteres        |                                                          |
| **ProvinceCode**      | Si            | Código A.F.I.P. con la cual se identifica la provincia del cliente.                                                                         | Alfanumérico de hasta 4 caracteres          | Ver Tablas de Referencia, [Provincias](#provincias).     |
| **PostalCode**        | No            | Código postal del domicilio del cliente                                                                                                     | Alfanumérico de hasta 8 caracteres          |                                                          |
| **PhoneNumber1**      | No            | Número de teléfono del cliente.                                                                                                             | Alfanumérico de hasta 30 caracteres         |                                                          |
| **PhoneNumber2**      | No            | Número de teléfono del cliente.                                                                                                             | Alfanumérico de hasta 30 caracteres         |                                                          |
| **IvaCategoryCode**   | Si            | Código de Categoría de I.V.A. del cliente                                                                                                   | Alfanumérico de hasta 3 caracteres          | Ver Tablas de Referencia, [Condición Fiscal](#cfiscal).  |
| **PayInternalTax**    | No            | Indica si se liquida impuestos internos (en caso de existir) al comprador                                                                   | De tipo lógico                              | True/False                                               |

<a name="topicocustomerhabitual"></a>
**_Como se relaciona con el cliente habitual_**

Si se informa el campo "Code" se va a utilizar este valor para buscar unívocamente al código de cliente en Tango.
En caso de no informarlo, para obtener la relación con el cliente habitual se realiza la siguiente búsqueda en orden de prioridad:

ABM Clientes – Solapa principal

• Tipo y número de documento

• Correo electrónico

ABM Clientes – Solapa contactos

• Tipo y número de documento

• Correo electrónico

• Usuario tienda

<a name="topicoordenitems"></a>
**Tópico OrderItems**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.

| **Campo**              | **Requerido** | **Descripción**                                                                                                               | **Tipo de Dato**                                                                                       | **Valores Posibles / Ejemplos**                                                                           |
| ---------------------- | ------------- | ----------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------- |
| **ProductCode**        | Si            | Código del artículo de la publicación.                                                                                        | Alfanumérico de hasta 200 caracteres                                                                   | &lt;&gt;Vacío. Debe ser único si la publicación no se trata de un artículo con escala.[Ejemplo](#Ejemplo) |
| **SKUCode**            | No            | Datos posibles: Código de artículo, Sinónimo o Código de barras registrados en el ABM de Artículos de Tango Gestión (se refiere al que se guarda en los campos STA11.Cod_Sta11, STA11.Sinonimo o STA11.Cod_Barra de las tablas de Tango Gestión) | Alfanumérico de hasta 40 caracteres                                                                    | [Ver nota](#VerNota)                                                                                      |
| **VariantCode**        | No            | Código del artículo que representa una combinación.                                                                           | Alfanumérico de hasta 200 caracteres                                                                   |                                                                                                           |
| **Description**        | Sí            | Descripción del artículo.                                                                                                     | Alfanumérico de hasta 400 caracteres                                                                   |                                                                                                           |
| **VariantDescription** | No            | Descripción del artículo que representa una variación.                                                                        | Alfanumérico de hasta 400 caracteres                                                                   |                                                                                                           |
| **Quantity**           | Si            | Cantidad del artículo.                                                                                                        | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0                                                                                                     |
| **DiscountPercentage**          | No            | Porcentaje de descuento.                                                                                                              | Numérico con 5 dígitos con hasta 2 decimales 99.99[.CC]. Usando el punto como separador de decimales | &gt;= 0 , <= 99.99                                                                                                          |
| **UnitPrice**          | Si            | Precio unitario.                                                                                                              | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales |                                                                                                           |
| **SelectMeasureUnit** | No            | Unidad de medida seleccionada| Alfanumérico de 1 caracter. | Los valores posibles de informar son V (Ventas), P (Stock 1) y S (Stock 2), en caso de no informarla se tomará Ventas por defecto. |
| **MeasureCode** | No            | Código de medida correspondiente del artículo. | Alfanumérico de 10 caracteres. | En caso de no informarlo se tomará vacío por defecto. |

<a name="VerNota"></a>

#### Nota: Código de artículo de Tango Gestión

• SKUCode: A través del dato informado, en caso de existir en Tango, se establecerá o actualizará la relación entre el artículo de la tienda (ProductCode) y el artículo de Tango Gestión (SKUCode).

<a name="Ejemplo"></a>
**Ejemplo de una publicación de artículos con escalas:**

```
"OrderItems":
[
   {
      "ProductCode": "010040",
      "SKUCode": "010040001RBL",
      "VariantCode": "BL",
      "Description": "TV",
      "VariantDescription": "TV BLANCO",
      "Quantity": 1.0,
      "UnitPrice": 500.0,
      "DiscountPercentage": 0.0
    },
    {
      "ProductCode": "010040",
      "SKUCode": "010040002NG",
      "VariantCode": "NG",
      "Description": "TV",
      "VariantDescription": "TV NEGRO",
      "Quantity": 1.0,
      "UnitPrice": 1000.0,
      "DiscountPercentage": 0.0
    }
]
```

**Ejemplo de una publicación de artículos sin escalas:**

```
"OrderItems":
[
   {
      "ProductCode": "1000",
      "SKUCode": "0100100150",
      "VariantCode": " ",
      "Description": "TV",
      "VariantDescription": " ",
      "Quantity": 1.0,
      "UnitPrice": 500.0,
      "DiscountPercentage": 0.0
   },
   {
      "ProductCode": "2000",
      "SKUCode": "0100100150",
      "VariantCode": " ",
      "Description": "TV",
      "VariantDescription": " ",
      "Quantity": 1.0,
      "UnitPrice": 1000.0,
      "DiscountPercentage": 0.0
   }
]
```

**Ejemplo de una publicación de artículos aplicando "Porcentaje de Descuentos:**

```
"OrderItems":
[
   {
      "ProductCode": "1000",
      "SKUCode": "0100100150",
      "VariantCode": " ",
      "Description": "TV",
      "VariantDescription": " ",
      "Quantity": 1.0,
      "UnitPrice": 500.0,
      "DiscountPercentage": 15.0
   }
]
```

#### Nota: Porcentaje de descuento

• Cálculo del **Precio por renglón** : al asignar un porcentaje de descuento, el mismo, se aplica de la siguiente manera:

```
	PRECIO POR RENGLON = (PRECIO UNITARIO - (PRECIO UNITARIO / 100 * PORCENTAJE DE DESCUENTO)) * CANTIDAD 
```

<a name="topicoshipping"></a>
**Tópico Shipping**

Este tópico se completa siempre que se requiere informar el envío. Se puede completar ya sea que el envío sea con o sin costo para el comprador.

| **Campo**             | **Requerido** | **Descripción**                                                     | **Tipo de Dato**                                                                                        | **Valores Posibles / Ejemplos**                                 |
| --------------------- | ------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------- |
| **ShippingID**        | Si            | Identificador del envío. Debe ser distinto para cada operación.     | Numérico de tipo entero hasta 50 posiciones.                                                            | &gt;0                                                           |
| **ShippingCode**      | No            | Código de la dirección de entrega.                                  | Alfanumérico de hasta 40 caracteres.                                                                    |                                                                 |
| **ShippingCost**      | No            | Importe correspondiente al costo de envío.                          | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales. | &gt;=0                                                          |
| **Street**            | No            | Calle del domicilio del cliente.                                    | Alfanumérico de hasta 200 caracteres                                                                    |                                                                 |
| **HouseNumber**       | No            | Altura del domicilio del cliente.                                   | Alfanumérico de hasta 200 caracteres                                                                    |                                                                 |
| **Floor**             | No            | Piso del domicilio del cliente.                                     | Alfanumérico de hasta 200 caracteres                                                                    |                                                                 |
| **Apartment**         | No            | Departamento del domicilio del cliente.                             | Alfanumérico de hasta 200 caracteres                                                                    |                                                                 |
| **City**              | No            | Localidad del domicilio del cliente.                                | Alfanumérico de hasta 200 caracteres                                                                    |                                                                 |
| **PostalCode**        | No            | Código postal de la dirección del cliente.                          | Alfanumérico de hasta 8 caracteres.                                                                     |                                                                 |
| **ProvinceCode**      | Si            | Código A.F.I.P. con la cual se identifica la provincia del cliente. | Alfanumérico de hasta 4 caracteres.                                                                     | Ver Tablas de Referencia, [Provincias](#provincias).            |
| **PhoneNumber1**      | No            | Número de teléfono del cliente.                                     | Alfanumérico de hasta 30 caracteres                                                                     |                                                                 |
| **PhoneNumber2**      | No            | Número de teléfono del cliente.                                     | Alfanumérico de hasta 30 caracteres                                                                     |                                                                 |
| **DeliversMonday**    | No            | Entrega lunes                                                       | Alfanumérico de hasta 1 caracteres                                                                      | [S/N] Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversTuesday**   | No            | Entrega martes                                                      | Alfanumérico de hasta 1 caracteres                                                                      | [S/N] Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversWednesday** | No            | Entrega miércoles                                                   | Alfanumérico de hasta 1 caracteres                                                                      | [S/N] Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversThursday**  | No            | Entrega jueves                                                      | Alfanumérico de hasta 1 caracteres                                                                      | [S/N] Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversFriday**    | No            | Entrega viernes                                                     | Alfanumérico de hasta 1 caracteres                                                                      | [S/N] Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversSaturday**  | No            | Entrega sábado                                                      | Alfanumérico de hasta 1 caracteres                                                                      | [S/N] Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversSunday**    | No            | Entrega domingo                                                     | Alfanumérico de hasta 1 caracteres                                                                      | [S/N] Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliveryHours**     | No            | Hora de entrega                                                     | Alfanumérico de hasta 100 caracteres                                                                    |                                                                 |
| **DeliveryDate**      | No            | Fecha de entrega                                                     | Datetime                                                                                              | La fecha a informar no podrá ser anterior a la fecha de la orden. Si se deja vacío tomará la fecha del día en que se genera el pedido o el plazo definido en los Parámetros de Ventas para la entrega de pedidos.                         |

**Consideraciones en la dirección de entrega**

Al informar el código de dirección de entrega de un cliente habitual, el cual se obtiene del recurso ["Customer"](#iniciorecursos), se deberá tener las siguientes consideraciones:

• Si existe en clientes habituales: será la dirección con la cual se generá el pedido y no se requiere completar el resto de los campos.

• Si NO existe en clientes habituales: se utilizará la dirección de entrega habitual que posea el cliente y no se requiere completar el resto de los campos.

• Si es vacío: se comportará como antes, debiendo completar el resto de los campos y validando el ingreso de "ProvinceCode".

Estas consideraciones sólo se aplican para aquellos casos donde se informan los datos de un cliente habitual.

<a name="topicocashpayments"></a>
**Tópico CashPayments**

**IMPORTANTE**: este tópico da soporte a una lista de CashPayment y reemplazará al tópico CashPayment. No se permite el uso simultáneo de ambos tópicos. Si utiliza actualmente el tópico CashPayment, se sugiere incluir dicha información en un ítem de esta nueva lista.

_Recuerde_: si no carga un registro en Payments, CashPayments (en reemplazo de CashPayment) o ambos, deberá completar la forma de cobro al momento de emitir la factura. Por otro lado, si lo que se envia es una modificación de una órden, la cual antes contenía el tópico CashPayment y ahora no, se procederá a cancelar el pago anterior.

| **Campo**         | **Requerido** | **Descripción**                                                                                                   | **Tipo de Dato**                                                                                        | **Valores Posibles / Ejemplos**                     |
| ----------------- | ------------- | ----------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | --------------------------------------------------- |
| **PaymentID**     | Si            | Identificador del pago. Debe ser distinto para cada operación. Incluso cuando se utiliza conjuntamente con el Tópico Payments si se combina con tarjetas. | Numérico de tipo entero hasta 50 posiciones.                                                            | &gt;0                                               |
| **PaymentMethod** | Si            | Código de Forma de Pago.                                                                                          | Alfanumérico de hasta 3 caracteres.                                                                     | Ver Tablas de Referencia, [Formas de Pago](#fpago). |
| **PaymentTotal**  | Si            | Total, del pago.                                                                                                  | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales. | &gt;0                                               |

<a name="topicocashpayment"></a>
**Tópico CashPayment**

**IMPORTANTE**: este tópico será reemplazado por el tópico CashPayments (en reemplazo de CashPayment). No se permite el uso simultáneo de ambos tópicos. Si utiliza actualmente este tópico, se sugiere incluir su información en un ítem del nuevo tópico CashPayments.

_Recuerde_: si no carga un registro en Payments, CashPayment o ambos, deberá completar la forma de cobro al momento de emitir la factura. Por otro lado, si lo que se envia es una modificación de una órden la cual antes contenía el tópico CashPayments y ahora no, se procederá a cancelar el pago anterior.

| **Campo**         | **Requerido** | **Descripción**                                                                                                   | **Tipo de Dato**                                                                                        | **Valores Posibles / Ejemplos**                     |
| ----------------- | ------------- | ----------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | --------------------------------------------------- |
| **PaymentID**     | Si            | Identificador del pago. Debe ser distinto para cada operación. Incluso cuando se utiliza conjuntamente con el tópico Payments si se combina con tarjetas. | Numérico de tipo entero hasta 50 posiciones.                                                            | &gt;0                                               |
| **PaymentMethod** | Si            | Código de Forma de Pago.                                                                                          | Alfanumérico de hasta 3 caracteres.                                                                     | Ver Tablas de Referencia, [Formas de Pago](#fpago). |
| **PaymentTotal**  | Si            | Total, del pago.                                                                                                  | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales. | &gt;0                                               |

<a name="topicopayments"></a>
**Tópico Payments**

_Recuerde_: si no carga un registro en Payments, CashPayments (en reemplazo de CashPayment) o ambos, deberá completar la forma de cobro al momento de emitir la factura. Por otro lado, si lo que se envia es una modificación de una órden la cual antes contenía un pago que ahora no, se procederá a cancelar el pago anterior no enviado en la modificación.

| **Campo**              | **Requerido** | **Descripción**                                                                                                  | **Tipo de Dato**                                                                                       | **Valores Posibles / Ejemplos**                                                                                                                          |
| ---------------------- | ------------- | ---------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **PaymentId**         | Si            | Identificador del pago. Debe ser distinto para cada operación. Incluso con PaymentID si se combina con efectivo. | Numérico de tipo entero hasta 50 posiciones.                                                           | &gt;0                                                                                                                                                    |
| **TransactionDate**    | Si            | Fecha en que se realizó el pago.                                                                                 | Datetime                                                                                               | &gt;Principal.Date yyyy-MM-ddTHH:mm:ss                                                                                                                   |
| **AuthorizationCode**  | No            | Código de autorización del pago de tarjeta.                                                                      | Alfanumérico de hasta 8 caracteres                                                                     |                                                                                                                                                          |
| **TransactionNumber**  | No            | Número de transacción de pago.                                                                                   | Alfanumérico de hasta 40 caracteres                                                                    |                                                                                                                                                          |
| **Installments**       | Si            | Cantidad de cuotas.                                                                                              | Numérico hasta 2 posiciones                                                                            | &gt;0                                                                                                                                                    |
| **InstallmentsAmount** | Si            | Importe correspondiente a la cuota.                                                                              | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0                                                                                                                                                    |
| **Total**              | Si            | Total, del pago.                                                                                                 | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0Installments \* InstallmentsAmount                                                                                                                  |
| **CardCode**           | Si            | Código de la tarjeta de crédito.                                                                                 | Alfanumérico de hasta 3 caracteres                                                                     | Código de la tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Tarjetas.              |
| **CardPlanCode**       | Si            | Plan de la tarjeta de crédito.                                                                                   | Alfanumérico de hasta 10 caracteres                                                                    | Código del plan de tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Planes.          |
| **VoucherNo**          | Si            | Número de cupón de tarjeta de crédito.                                                                           | Numérico hasta 8 posiciones                                                                            | &gt;0                                                                                                                                                    |
| **CardPromotionCode**  | No            | Código de promoción de la tarjeta de crédito.                                                                    | Alfanumérico de hasta 10 caracteres                                                                    | Código de promoción de tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Promociones. |


<a name="lote"></a>

### Órdenes por Lote

[<sub>Volver</sub>](#inicio)

Recepción de órdenes en forma masiva 

La URL del servicio de API para órdenes en lote es: 

https://tiendas.axoft.com/api/Aperture/order/batch 

Si desea enviar órdenes en forma masiva, puede realizar una llamada utilizando el método POST al recurso, con el siguiente formato: 

```
{  
 "OrderBatch":
    [
	{ 
            "OrderId": 1…. 
        }, 
        { 
            "OrderId": 2…. 
        }, 
        { 
            "OrderId": 3…. 
        } 
    ] 
} 
```
 
Tenga en cuenta que el número máximo de órdenes a enviar por lote es de 25. 

**Response** 

Una vez procesado el lote, el response devolverá un json en el campo data con los ids generados y los errores obtenidos en caso de que los hubiera. 

 
Ejemplo  

```
{
   "Status":0,
   "Message":"batch processed",
   "Data":{
      "Results":[
         {
            "OrderID":"116081",
            "Inprocess":true
         },
         {
            "OrderID":"116082",
            "Inprocess":false,
            "ValidationException":"Order total doesn't add up."
         }
      ]
   },
   "isOk":false
}
```

<a name="tablas"></a>

### Tablas de Referencia

[<sub>Volver</sub>](#inicio)

<a name="tipodoc"></a>

#### Tipo de Documento

| **Código** | **Descripción** |
| ---------- | --------------- |
| 80         | C.U.I.T.        |
| 86         | C.U.I.L.        |
| 87         | C.D.I.          |
| 89         | L.E.            |
| 90         | L.C.            |
| 96         | D.N.I.          |

<a name="provincias"></a>

#### Provincias

| **Código** | **Descripción**              |
| ---------- | ---------------------------- |
| 0          | CIUDAD AUTONOMA BUENOS AIRES |
| 1          | BUENOS AIRES                 |
| 2          | CATAMARCA                    |
| 3          | CORDOBA                      |
| 4          | CORRIENTES                   |
| 5          | ENTRE RIOS                   |
| 6          | JUJUY                        |
| 7          | MENDOZA                      |
| 8          | LA RIOJA                     |
| 9          | SALTA                        |
| 10         | SAN JUAN                     |
| 11         | SAN LUIS                     |
| 12         | SANTA FE                     |
| 13         | SANTIAGO DEL ESTERO          |
| 14         | TUCUMAN                      |
| 16         | CHACO                        |
| 17         | CHUBUT                       |
| 18         | FORMOSA                      |
| 19         | MISIONES                     |
| 20         | NEUQUEN                      |
| 21         | LA PAMPA                     |
| 22         | RIO NEGRO                    |
| 23         | SANTA CRUZ                   |
| 24         | TIERRA DEL FUEGO             |

<a name="cfiscal"></a>

#### Condición Fiscal

| **Código** | **Descripción**                          |
| ---------- | ---------------------------------------- |
| CF         | CONSUMIDOR FINAL                         |
| EX         | EXENTO                                   |
| EXE        | EXENTO OPERACIÓN EXPORTACIÓN             |
| INR        | NO RESPONSABLE                           |
| RI         | RESPONSABLE INSCRIPTO                    |
| RS         | RESPONSABLE MONOTRIBUTISTA               |
| RSS        | RESPONSABLE MONOTRIBUTISTA SOCIAL        |
| PCE        | PEQUEÑO CONTRIBUYENTE EVENTUAL           |
| PCS        | PEQUEÑO CONTRIBUYENTE EVENTUAL SOCIAL    |
| SNC        | SUJETO NO CATEGORIZADO                   |

<a name="fpago"></a>

#### Formas de Pago

| **Código** | **Descripción**           |
| ---------- | ------------------------- |
| A01        | Forma de cobro Web API 01 |
| A02        | Forma de cobro Web API 02 |
| A03        | Forma de cobro Web API 03 |
| A04        | Forma de cobro Web API 04 |
| A05        | Forma de cobro Web API 05 |
| A06        | Forma de cobro Web API 06 |
| A07        | Forma de cobro Web API 07 |
| A08        | Forma de cobro Web API 08 |
| A09        | Forma de cobro Web API 09 |
| A10        | Forma de cobro Web API 10 |
| MPA        | MercadoPago Argentina     |
| PPA        | PayPal Argentina          |
| PUA        | PayU Argentina            |
| TPA        | Todo Pago Argentina       |

<a name="ejemplojson"></a>

### Ejemplo de JSON de una órden (Condición de venta - Contado)

[<sub>Volver</sub>](#inicio)

```
{
  "Date": "2020-02-14T00:00:00",
  "Total": 8523.0,
  "TotalDiscount": 77.0,
  "PaidTotal": 8523.0,
  "FinancialSurcharge": 200.0,
  "WarehouseCode": "2",
  "SellerCode": "2",
  "TransportCode": "01",
  "SaleConditionCode": 1,
  "InvoiceCounterfoil": 1,
  "OrderID": "75906",
  "OrderNumber": "75906",
  "ValidateTotalWithPaidTotal": true,
  "Customer": {
    "CustomerID": 227060905,
    "DocumentType": "80",
    "DocumentNumber": "11111111111",
    "IVACategoryCode": "CF",
    "User": "ADMIN",
    "Email": "api@axoft.com",
    "FirstName": "Carlos",
    "LastName": "Perez",
    "BusinessName": "Empresa",
    "Street": "Cerrrito",
    "HouseNumber": "1186",
    "Floor": "2",
    "Apartment": "1",
    "City": "CABA",
    "ProvinceCode": "0",
    "PostalCode": "1122",
    "PhoneNumber1": "12459856",
    "PhoneNumber2": "42563698",
    "Bonus": 0.0,
    "MobilePhoneNumber": "165952141",
    "WebPage": null,
    "BusinessAddress": "Cerrito 1186",
    "Comments": "Comentario",
    "NumberListPrice": 0,
    "Removed": false,
    "DateUpdate": "0001-01-01T00:00:00",
    "Disable": "0001-01-01T00:00:00"
  },
  "CancelOrder": false,
  "OrderItems": [
    {
      "ProductCode": "203",
      "SKUCode": "0100200659",
      "VariantCode": null,
      "Description": "LAVARROPAS AUTOM. MOD.BLUE ",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 7700.0,
      "DiscountPercentage": 0.0
    },
    {
      "ProductCode": "104",
      "SKUCode": "0100100269",
      "VariantCode": null,
      "Description": "CÁMARA DIGITAL 4X MARCA TCL",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 300.0,
      "DiscountPercentage": 0.0
    }
  ],
  "Shipping": {
    "ShippingID": 71906,
    "Street": "9 de Julio",
    "HouseNumber": "1186",
    "Floor": "1",
    "Apartment": "1",
    "City": "CABA",
    "ProvinceCode": "0",
    "PostalCode": "1122",
    "PhoneNumber1": "125165151",
    "PhoneNumber2": "12345678",
    "ShippingCost": 400.0,
    "DeliversMonday": "S",
    "DeliversTuesday": "S",
    "DeliversWednesday": "S",
    "DeliversThursday": "S",
    "DeliversFriday": "S",
    "DeliversSaturday": "S",
    "DeliversSunday": "S",
    "DeliveryHours": "8"
  },
  "CashPayments": [
	{
	"PaymentID": 38566912,
	"PaymentMethod": "A02",
	"PaymentTotal": 123.0
	}
  ],
  "Payments": [
    {
      "PaymentId": 38566913,
      "TransactionDate": "2020-02-14T00:00:00",
      "AuthorizationCode": "52",
      "TransactionNumber": "998595",
      "Installments": 1,
      "InstallmentAmount": 8100.0,
      "Total": 8100.0,
      "CardCode": "DI",
      "CardPlanCode": "1",
      "VoucherNo": 48,
      "CardPromotionCode": "2"
    },
    {
      "PaymentId": 38566914,
      "TransactionDate": "2020-02-14T00:00:00",
      "AuthorizationCode": "53",
      "TransactionNumber": "5849849",
      "Installments": 2,
      "InstallmentAmount": 150.0,
      "Total": 300.0,
      "CardCode": "DI",
      "CardPlanCode": "2",
      "VoucherNo": 49,
      "CardPromotionCode": "1"
    }
  ]
}
```

### Ejemplo de JSON de una órden (Condición de venta - Cuenta Corriente)

[<sub>Volver</sub>](#inicio)

```
{
  "Date": "2020-05-28T00:00:00",
  "Total": 8400.0,
  "TotalDiscount": 0.0,
  "PaidTotal": 0.0,
  "FinancialSurcharge": 0.0,
  "WarehouseCode": "2",
  "SellerCode": "2",
  "TransportCode": "02",
  "SaleConditionCode": 3,
  "InvoiceCounterfoil": 2,
  "OrderID": "75906",
  "OrderNumber": "75906",
  "ValidateTotalWithPaidTotal": false,
  "Customer": {
    "CustomerID": 227060905,
    "Code": null,
    "DocumentType": "80",
    "DocumentNumber": "11111111111",
    "IVACategoryCode": "CF",
    "User": "ADMIN",
    "Email": "api@axoft.com",
    "FirstName": "Carlos",
    "LastName": "Perez",
    "BusinessName": "Empresa",
    "Street": "Cerrrito",
    "HouseNumber": "1186",
    "Floor": "2",
    "Apartment": "1",
    "City": "CABA",
    "ProvinceCode": "0",
    "PostalCode": "1122",
    "PhoneNumber1": "12459856",
    "PhoneNumber2": "42563698",
    "Bonus": 0.0,
    "MobilePhoneNumber": "165952141",
    "WebPage": null,
    "BusinessAddress": "Cerrito 1186",
    "Comments": "Comentario",
    "NumberListPrice": 0,
    "Removed": false,
    "DateUpdate": "0001-01-01T00:00:00",
    "Disable": "0001-01-01T00:00:00"
  },
  "CancelOrder": false,
  "OrderItems": [
    {
      "ProductCode": "203",
      "SKUCode": "0100200659",
      "VariantCode": null,
      "Description": "LAVARROPAS AUTOM. MOD.BLUE ",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 8000.0,
      "DiscountPercentage": 0.0
    }
  ],
  "Shipping": {
    "ShippingID": 71906,
    "Street": "9 de Julio",
    "HouseNumber": "1186",
    "Floor": "1",
    "Apartment": "1",
    "City": "CABA",
    "ProvinceCode": "0",
    "PostalCode": "1122",
    "PhoneNumber1": "125165151",
    "PhoneNumber2": "12345678",
    "ShippingCost": 400.0,
    "DeliversMonday": "S",
    "DeliversTuesday": "S",
    "DeliversWednesday": "S",
    "DeliversThursday": "S",
    "DeliversFriday": "S",
    "DeliversSaturday": "S",
    "DeliversSunday": "S",
    "DeliveryHours": "8"
  },
  "CashPayment": null,
  "Payments": []
}
```

### Ejemplo de JSON de una órden (Código de dirección de entrega)

[<sub>Volver</sub>](#inicio)

```
{
  "Date": "2020-05-28T00:00:00",
  "Total": 8400.0,
  "TotalDiscount": 0.0,
  "PaidTotal": 0.0,
  "FinancialSurcharge": 0.0,
  "WarehouseCode": "2",
  "SellerCode": "2",
  "TransportCode": "02",
  "SaleConditionCode": 3,
  "InvoiceCounterfoil": 3,
  "OrderID": "75906",
  "OrderNumber": "75906",
  "ValidateTotalWithPaidTotal": false,
  "Customer": {
    "CustomerID": 227060905,
    "Code": null,
    "DocumentType": "80",
    "DocumentNumber": "11111111111",
    "IVACategoryCode": "CF",
    "User": "ADMIN",
    "Email": "api@axoft.com",
    "FirstName": "Carlos",
    "LastName": "Perez",
    "BusinessName": "Empresa",
    "Street": "Cerrrito",
    "HouseNumber": "1186",
    "Floor": "2",
    "Apartment": "1",
    "City": "CABA",
    "ProvinceCode": "0",
    "PostalCode": "1122",
    "PhoneNumber1": "12459856",
    "PhoneNumber2": "42563698",
    "Bonus": 0.0,
    "MobilePhoneNumber": "165952141",
    "WebPage": null,
    "BusinessAddress": "Cerrito 1186",
    "Comments": "Comentario",
    "NumberListPrice": 0,
    "Removed": false,
    "DateUpdate": "0001-01-01T00:00:00",
    "Disable": "0001-01-01T00:00:00"
  },
  "CancelOrder": false,
  "OrderItems": [
    {
      "ProductCode": "203",
      "SKUCode": "0100200659",
      "VariantCode": null,
      "Description": "LAVARROPAS AUTOM. MOD.BLUE ",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 8000.0,
      "DiscountPercentage": 0.0
    }
  ],
  "Shipping": {
    "ShippingID": 71906,
    "ShippingCode": "PRINCIPAL",
    "Street": "",
    "HouseNumber": "",
    "Floor": "",
    "Apartment": "",
    "City": "",
    "ProvinceCode": null,
    "PostalCode": "",
    "PhoneNumber1": "",
    "PhoneNumber2": "",
    "ShippingCost": 400.0,
    "DeliversMonday": "",
    "DeliversTuesday": "",
    "DeliversWednesday": "",
    "DeliversThursday": "",
    "DeliversFriday": "",
    "DeliversSaturday": "",
    "DeliversSunday": "",
    "DeliveryHours": ""
  },
  "CashPayments": [],
  "Payments": []
}
```

### Ejemplo de JSON de una órden (Código de Cliente - Lista de Precio)

[<sub>Volver</sub>](#inicio)

```
{
  "Date": "2020-02-14T00:00:00",
  "Total": 8523.0,
  "TotalDiscount": 77.0,
  "PaidTotal": 8523.0,
  "FinancialSurcharge": 200.0,
  "WarehouseCode": "2",
  "SellerCode": "2",
  "TransportCode": "01",
  "SaleConditionCode": 1,
  "InvoiceCounterfoil": 2,
  "PriceListNumber": 2,
  "IvaIncluded": true,
  "InternalTaxIncluded": false,
  "OrderID": "75906",
  "OrderNumber": "75906",
  "ValidateTotalWithPaidTotal": true,
  "Customer": {
    "CustomerID": 227060905,
    "Code": "010010",
    "DocumentType": "80",
    "DocumentNumber": "11111111111",
    "IVACategoryCode": "CF",
    "PayInternalTax": false,
    "User": "ADMIN",
    "Email": "api@axoft.com",
    "FirstName": "Carlos",
    "LastName": "Perez",
    "BusinessName": "Empresa",
    "Street": "Cerrrito",
    "HouseNumber": "1186",
    "Floor": "2",
    "Apartment": "1",
    "City": "CABA",
    "ProvinceCode": "0",
    "PostalCode": "1122",
    "PhoneNumber1": "12459856",
    "PhoneNumber2": "42563698",
    "Bonus": 0.0,
    "MobilePhoneNumber": "165952141",
    "WebPage": null,
    "BusinessAddress": "Cerrito 1186",
    "Comments": "Comentario",
    "NumberListPrice": 0,
    "Removed": false,
    "DateUpdate": "0001-01-01T00:00:00",
    "Disable": "0001-01-01T00:00:00"
  },
  "CancelOrder": false,
  "OrderItems": [
    {
      "ProductCode": "203",
      "SKUCode": "0100200659",
      "VariantCode": null,
      "Description": "LAVARROPAS AUTOM. MOD.BLUE ",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 7700.0,
      "DiscountPercentage": 0.0
    },
    {
      "ProductCode": "104",
      "SKUCode": "0100100269",
      "VariantCode": null,
      "Description": "CÁMARA DIGITAL 4X MARCA TCL",
      "VariantDescription": null,
      "Quantity": 1.0,
      "UnitPrice": 300.0,
      "DiscountPercentage": 0.0
    }
  ],
  "Shipping": {
    "ShippingID": 71906,
    "Street": "9 de Julio",
    "HouseNumber": "1186",
    "Floor": "1",
    "Apartment": "1",
    "City": "CABA",
    "ProvinceCode": "0",
    "PostalCode": "1122",
    "PhoneNumber1": "125165151",
    "PhoneNumber2": "12345678",
    "ShippingCost": 400.0,
    "DeliversMonday": "S",
    "DeliversTuesday": "S",
    "DeliversWednesday": "S",
    "DeliversThursday": "S",
    "DeliversFriday": "S",
    "DeliversSaturday": "S",
    "DeliversSunday": "S",
    "DeliveryHours": "8"
  },
  "CashPayments": [
	{
	"PaymentID": 38566912,
	"PaymentMethod": "A02",
	"PaymentTotal": 123.0
	}
  ],
  "Payments": [
    {
      "PaymentId": 38566913,
      "TransactionDate": "2020-02-14T00:00:00",
      "AuthorizationCode": "52",
      "TransactionNumber": "998595",
      "Installments": 1,
      "InstallmentAmount": 8100.0,
      "Total": 8100.0,
      "CardCode": "DI",
      "CardPlanCode": "1",
      "VoucherNo": 48,
      "CardPromotionCode": "2"
    },
    {
      "PaymentId": 38566914,
      "TransactionDate": "2020-02-14T00:00:00",
      "AuthorizationCode": "53",
      "TransactionNumber": "5849849",
      "Installments": 2,
      "InstallmentAmount": 150.0,
      "Total": 300.0,
      "CardCode": "DI",
      "CardPlanCode": "2",
      "VoucherNo": 49,
      "CardPromotionCode": "1"
    }
  ]
}
```

<a name="subida"></a>

## Consulta de datos

[<sub>Volver</sub>](#inicio)

La consulta de datos se basa en una serie de servicios que permiten consultar datos obtenidos de **Tango Gestión o Tango Punto de Venta** y devolviendo como resultado, la respuesta en formato JSON.

<a name="configuracion"></a>

### Configuración

[<sub>Volver</sub>](#inicio)

Previo a comenzar a utilizar los servicios de consulta debe verificar la configuración de **Tango Tiendas** desde su sistema **Tango Gestión o Tango Punto de Venta**, accediendo al wizard de la aplicación de Nexo > Tiendas . Allí debe indicar mediante la configuración la información que desea sincronizar. Tenga en cuenta que, si centraliza stock de varias sucursales, y desea informar a tiendas los saldos centralizados, deberá seleccionar la opción "Incluir información de saldos de otras sucursales (Inf. y estadísticas)".

Se debe tener en cuenta que los artículos se obtienen siempre de la sucursal asociada, por lo cual si se centralizan saldos de stock es necesario que todos los artículos de las diferentes sucursales hayan sido importados en la sucursal asociada.

![imagenapifull](https://github.com/TangoSoftware/ApiTiendas/blob/master/apifull.jpg)

<a name="ConceptosBasicos"></a>

### Conceptos básicos

[<sub>Volver</sub>](#inicio)

#### Caracteristicas de la URL

La consulta se entregará de forma paginada, para lo cual se debe realizar una petición por página que tenga el resultado. Se debe comenzar por la página 1 (pageSize=1)

Las url de los métodos tendrán el siguiente formato:

```
https://tiendas.axoft.com/api/Aperture/Store?pageSize=500&pageNumber=1&filter=1
```

| **Sección** | **Obligatorio** | **Descripción**                                                                             |
| ----------- | --------------- | ------------------------------------------------------------------------------------------- |
| pageSize    | Si              | Indica la cantidad de ítems a mostrar por página (No podrá ser superior a 5000)             |
| pageNumber  | Si              | Indica la página solicitada                                                                 |
| filter      | No              | Indica el valor por el cual filtrar la consulta (Si no se coloca, no se aplicarán filtros.) |

El resultado contiene dos secciones, **Paging**, que muestra información acerca de la página y el conteo, y **Data**, que muestra los datos requeridos en la petición.

```
"Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
},
```

| **Dato**   | **Descripción**                                     |
| ---------- | --------------------------------------------------- |
| PageNumber | Indica el número de página actual                   |
| PageSize   | Indica el tamaño de página indicado en la solicitud |
| MoreData   | Indica si existe una página siguiente con datos     |

<a name="iniciorecursos"></a>
<a name="recursos"></a>

### Recursos de consulta

[<sub>Volver</sub>](#inicio)

- [Sucursales](#sucursales)
- [Depósitos](#depositos)
- [Unidades de medida](#medidas)
- [Artículos](#articulos)
- [Artículos por depósito y saldo de stock](#ArticulosDepositoSaldoStock)
- [Clientes](#clientes)
- [Listas de precios](#listaprecios)
- [Precios](#precios)
- [Precios por cliente](#precioscliente)
- [Descuentos por cliente](#descuentoscliente)
- [Saldos de stock](#saldosstock)
- [Vendedores](#vendedores)
- [Monedas](#monedas)
- [Transportes](#transportes)
- [Condiciones de venta](#condicionesventa)
- [Clasificador de artículos](#ClasificadorArticulos)
  - [Carpetas de artículos](#CarpetasClasificadorArticulos)
  - [Artículos en las carpetas](#ArticulosEnCarpetaClasificador)
  - [Relaciones](#ArticulosRelacionadosClasificador)
- [Clasificador de clientes](#ClasificadorClientes)
  - [Carpetas de clientes](#CarpetasClasificadorClientes)
  - [Clientes en las carpetas](#ClientesEnCarpetaClasificador)
  - [Relaciones](#ClientesRelacionadosClasificador)
- [Cotización de moneda extranjera](#CotizacionMonedaExtranjera)
- [Publicaciones](#Publicaciones)
- [Comprobantes de facturación](#ComprobantesDeFacturacion)
- [Talonarios](#Talonarios)
- [Estado de órdenes](#EstadosOrdenes)

<a name="sucursales"></a>

#### Sucursales

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de sucursales.

| **Recurso**                                                                   |
| ----------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Store?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                         | **GET**                                                                         |
| ------------------------------------------------ | ------------------------------------------------------------------------------- |
| Obtener la sucursal cuyo número de sucursal es 2 | https://tiendas.axoft.com/api/Aperture/Store?pageSize=500&pageNumber=1&filter=2 |
| Obtener todas las sucursales                     | https://tiendas.axoft.com/api/Aperture/Store?pageSize=500&pageNumber=1          |

Respuesta

_Recuerde_:El código de provincia informado corresponde con la tabla de provincias proporcionada por AFIP. ([Tablas de Referencia](#tablas))

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "StoreNumber": 1,
            "Description": "CASA CENTRAL",
            "Street": "",
            "Number": "",
            "Floor": "",
            "Apartment": "",
            "Tower": "",
            "Block": "",
            "City": "",
            "PostalCode": "",
            "ProvinceCode": "",
            "Email": "",
            "WebPage": "",
            "Contact": "PRUEBA",
            "PhoneNumber1": "(33333)3333-3333",
            "PhoneNumber2": "(22222)2222-2222"
        },
        {
            "StoreNumber": 2,
            "Description": "MAR DEL PLATA",
            "Street": "",
            "Number": "",
            "Floor": "",
            "Apartment": "",
            "Tower": "",
            "Block": "",
            "City": "",
            "PostalCode": "",
            "ProvinceCode": "",
            "Email": "",
            "WebPage": "",
            "Contact": "",
            "PhoneNumber1": "(22222)2222-2222",
            "PhoneNumber2": "(88888)8888-8888"
        }
    ]
}
```

<a name="depositos"></a>

#### Depósitos

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de depósitos.

| **Recurso**                                                                       |
| --------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Warehouse?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                                  | **GET**                                                                              |
| --------------------------------------------------------- | ------------------------------------------------------------------------------------ |
| Obtener los depósitos cuyo código contenga la cadena "01" | https://tiendas.axoft.com/api/Aperture/Warehouse?pageSize=500&pageNumber=1&filter=01 |
| Obtener todos los depósitos                               | https://tiendas.axoft.com/api/Aperture/Warehouse?pageSize=500&pageNumber=1           |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "Code": "1",
            "Description": "DEPOSITO CASA CENTRAL1",
            "Disabled": false
        },
        {
            "Code": "2",
            "Description": "DEPOSITO GALPON1",
            "Disabled": false
        },
        {
            "Code": "80",
            "Description": "DEPOSITO PRUEBA",
            "Disabled": false
        }
    ]
}
```

<a name="medidas"></a>

#### Unidades de medida

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de unidades de medida.

| **Recurso**                                                                     |
| ------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Measure?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                                | **GET**                                                                            |
| ------------------------------------------------------- | ---------------------------------------------------------------------------------- |
| Obtener las medidas cuyo código contenga la cadena "KG" | https://tiendas.axoft.com/api/Aperture/Measure?pageSize=500&pageNumber=1&filter=KG |
| Obtener todas las medidas                               | https://tiendas.axoft.com/api/Aperture/Measure?pageSize=500&pageNumber=1           |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 10,
        "MoreData": false
    },
    "Data": [
        {
            "Code": "M",
            "Initials": "MTS",
            "Description": "Metros"
        },
        {
            "Code": "UNI",
            "Initials": "UN",
            "Description": "Unidades"
        }
    ]
}
```

<a name="articulos"></a>

#### Artículos

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de artículos, con su composición, comentarios y valores de escala.
Nota: Si tenia implementado la consulta de API, para visualizar las descripciones de las escalas deberá realizar alguna modificación sobre el maestro de definición de escalas desde Tango.

Solo se mostrarán artículos que en **Tango Gestión** cumplan:

- Perfil de Venta, Compra-Venta o Inhabilitado.
- Tipo Simple, Fórmula, o Kit fijo.
- No sean artículos Base.

| **Recurso**                                                                                                                                        |
| -------------------------------------------------------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Product?&{pageSize}&{pageNumber}&{onlyEnabled}&{updatedDate}&{onlyKit}&{alternativeCode}&{barCode}&[filter] |

Ejemplos

| **Para**                                                                                                       | **GET**                                                                                                  |
| -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------- |
| Obtener los artículos cuyo código contenga la cadena "01"                                                      | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1&filter=01                       |
| Obtener sólo los artículos habilitados                                                                         | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1&onlyEnabled=true                |
| Obtener sólo los artículos Kit                                                                                 | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1&onlyKit=true                    |
| Obtener sólo los artículos con última actualización igual o posterior al 01/01/2020 a las 00:00:00 horas (UTC) | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1&updatedDate=1900-01-01T00:00:00 |
| Obtener el artículo cuyo sinónimo sea “EAZAP”                                                                  | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1&alternativeCode=EAZAP           |
| Obtener el artículo cuyo código de barras sea “01010101”                                                       | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1&barCode=01010101                |
| Obtener todos los artículos                                                                                    | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1                                 |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "SKUCode": "ART_EXPORTAC.",
            "Description": "ARTICULO DE EXPORTACION",
            "AdditionalDescription": "DESC. ADICIONAL A.E.",
            "AlternativeCode": "",
            "BarCode": "",
            "Commission": 6,
            "Discount": 0,
            "MeasureUnitCode": "UNI",
            "SecondMeasureUnitCode": "",
            "StockEquivalence": 0.0000000,
            "StockControlUnit": "P",
            "SalesMeasureUnitCode": "UNI",
            "SalesEquivalence": 1,
            "MaximumStock": 1000,
            "MinimumStock": 100,
            "RestockPoint": 300,
            "Observations": "",
            "Kit": false,
            "KitValidityDateSince": null,
            "KitValidityDateUntil": null,
            "UseScale": "N",
            "Scale1": "",
            "Scale2": "",
            "BaseArticle": "",
            "ScaleValue1": "",
            "ScaleValue2": "",
            "DescriptionScale1": "",
            "DescriptionScale2": "",
            "DescriptionValueScale1": "",
            "DescriptionValueScale2": "",
            "Disabled": false,
            "ProductComposition": [],
            "ProductComments": []
        },
        {
            "SKUCode": "ART_TIENDA",
            "Description": "",
            "AdditionalDescription": "",
            "AlternativeCode": "",
            "BarCode": "",
            "Commission": 6,
            "Discount": 0,
            "MeasureUnitCode": "UNI",
            "SecondMeasureUnitCode": "",
            "StockEquivalence": 0.0000000,
            "StockControlUnit": "P",
            "SalesMeasureUnitCode": "UNI",
            "SalesEquivalence": 1,
            "MaximumStock": 150,
            "MinimumStock": 10,
            "RestockPoint": 18,
            "Observations": "SIN OBSERVACIONES\r\n",
            "Kit": false,
            "KitValidityDateSince": null,
            "KitValidityDateUntil": null,
            "UseScale": "N",
            "Scale1": "",
            "Scale2": "",
            "BaseArticle": "",
            "ScaleValue1": "",
            "ScaleValue2": "",
            "DescriptionScale1": "",
            "DescriptionScale2": "",
            "DescriptionValueScale1": "",
            "DescriptionValueScale2": "",
            "Disabled": false,
            "ProductComposition": [],
            "ProductComments": [
                {
                    "Line": 1,
                    "Text": "Potencia RSM : 800 W\r\nCantidad de canales: 5.1\r\nControl remoto: Si\r\nFormatos: MP3\r\nRadio\r\nSalida de audio y video.\r\n"
                }
            ]
        },
        {
            "SKUCode": "ART01",
            "Description": "ART01",
            "AdditionalDescription": "",
            "AlternativeCode": "",
            "BarCode": "",
            "Commission": 6,
            "Discount": 0,
            "MeasureUnitCode": "UNI",
            "SecondMeasureUnitCode": "",
            "StockEquivalence": 0.0000000,
            "StockControlUnit": "P",
            "SalesMeasureUnitCode": "UNI",
            "SalesEquivalence": 1,
            "MaximumStock": 0,
            "MinimumStock": 0,
            "RestockPoint": 0,
            "Observations": "OBSERVACIONES",
            "Kit": false,
            "KitValidityDateSince": null,
            "KitValidityDateUntil": null,
            "UseScale": "N",
            "Scale1": "",
            "Scale2": "",
            "BaseArticle": "",
            "ScaleValue1": "",
            "ScaleValue2": "",
            "DescriptionScale1": "",
            "DescriptionScale2": "",
            "DescriptionValueScale1": "",
            "DescriptionValueScale2": "",
            "Disabled": false,
            "ProductComposition": [],
            "ProductComments": [
                {
                    "Line": 1,
                    "Text": "COMENTARIO"
                }
            ]
        },
        {
            "SKUCode": "KIT100",
            "Description": "KIT AUDIO COMPLETO",
            "AdditionalDescription": "",
            "AlternativeCode": "",
            "BarCode": "",
            "Commission": 6,
            "Discount": 0,
            "MeasureUnitCode": "UNI",
            "SecondMeasureUnitCode": "",
            "StockEquivalence": 0.0000000,
            "StockControlUnit": "P",
            "SalesMeasureUnitCode": "UNI",
            "SalesEquivalence": 1,
            "MaximumStock": 0,
            "MinimumStock": 0,
            "RestockPoint": 0,
            "Observations": "",
            "Kit": true,
            "KitValidityDateSince": null,
            "KitValidityDateUntil": null,
            "UseScale": "N",
            "Scale1": "",
            "Scale2": "",
            "BaseArticle": "",
            "ScaleValue1": "",
            "ScaleValue2": "",
            "DescriptionScale1": "",
            "DescriptionScale2": "",
            "DescriptionValueScale1": "",
            "DescriptionValueScale2": "",
            "Disabled": false,
            "ProductComposition": [
                {
                    "ComponentSKUCode": "0100100150",
                    "Quantity": 1
                },
                {
                    "ComponentSKUCode": "0100100151",
                    "Quantity": 2
                }
            ],
            "ProductComments": []
        },
        {
            "SKUCode": "KIT999",
            "Description": "KIT999",
            "AdditionalDescription": "NUEVO KIT 999",
            "AlternativeCode": "SIN999",
            "BarCode": "1222453364",
            "Commission": 6,
            "Discount": 0,
            "MeasureUnitCode": "UNI",
            "SecondMeasureUnitCode": "",
            "StockEquivalence": 0.0000000,
            "StockControlUnit": "P",
            "SalesMeasureUnitCode": "UNI",
            "SalesEquivalence": 1,
            "MaximumStock": 0,
            "MinimumStock": 0,
            "RestockPoint": 0,
            "Observations": "OBSERVACIONES",
            "Kit": true,
            "KitValidityDateSince": "2019-02-01T00:00:00",
            "KitValidityDateUntil": null,
            "UseScale": "N",
            "Scale1": "",
            "Scale2": "",
            "BaseArticle": "",
            "ScaleValue1": "",
            "ScaleValue2": "",
            "DescriptionScale1": "",
            "DescriptionScale2": "",
            "DescriptionValueScale1": "",
            "DescriptionValueScale2": "",
            "Disabled": false,
            "ProductComposition": [
                {
                    "ComponentSKUCode": "0100100150",
                    "Quantity": 2
                },
                {
                    "ComponentSKUCode": "0100100151",
                    "Quantity": 2
                }
            ],
            "ProductComments": [
                {
                    "Line": 1,
                    "Text": "COMENTARIO PARA LA IMPRESION"
                }
            ]
        },
        {
            "SKUCode": "ART_DOBLEUNIDAD",
            "Description": "ART.CON DOBLE UNIDAD DE MEDIDA",
            "AdditionalDescription": "",
            "AlternativeCode": "DBLEUNI",
            "BarCode": "",
            "Commission": 0.0000000,
            "Discount": 0.0000000,
            "MeasureUnitCode": "KILOGRAMOS",
            "SecondMeasureUnitCode": "UNI",
            "StockEquivalence": 3.0000000,
            "StockControlUnit": "P",
            "SalesMeasureUnitCode": "UNI",
            "SalesEquivalence": 1.0000000,
            "MaximumStock": 0.0000000,
            "MinimumStock": 0.0000000,
            "RestockPoint": 0.0000000,
            "Observations": "",
            "Kit": false,
            "KitValidityDateSince": null,
            "KitValidityDateUntil": null,
            "LastUpdateUtc": "2022-02-14T20:16:30.043",
            "UseScale": "N",
            "Scale1": "",
            "Scale2": "",
            "BaseArticle": "",
            "ScaleValue1": "",
            "ScaleValue2": "",
            "DescriptionScale1": null,
            "DescriptionScale2": null,
            "DescriptionValueScale1": null,
            "DescriptionValueScale2": null,
            "Disabled": false,
            "ProductComposition": [],
            "ProductComments": []
        }
    ]
}
```

<a name="ArticulosDepositoSaldoStock"></a>

#### Artículos por depósito y saldo de stock

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener por POST datos de artículos, con su composición, comentarios y valores de escala, pero filtrándolos por el stock disponible y su código de depósito. Nota: Si tenia implementado la consulta de API, para visualizar las descripciones de las escalas deberá realizar alguna modificación sobre el maestro de definición de escalas desde Tango.

Solo se mostrarán artículos que en **Tango Gestión** cumplan:

- Perfil de Venta, Compra-Venta o Inhabilitado.
- Tipo Simple, Fórmula, o Kit fijo.
- No sean artículos Base.

| **Recurso**                                                                            |
| -------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/DataBy/ArtPorSaldoStock?{pageSize}&{pageNumber} |

A diferencia del resto de recursos, requiere del envío de los parámetros de depósito y stock por body:

Impotante: El formato del body para los parámetros es JSON.

```
{
    "codigoDeposito": "2",
    "cantidadStock": 50
}
```

Ambos parámetros son obligatorios, y deben tener exactamente esos nombres. El primero es un string de dos caracteres que representa al código del depósito (ver [Depósitos](#depositos)) y busca por igualdad (=), mientras que el segundo es un decimal y busca por mayor estricto (>).

| **Para**                                                                | **POST**                                                                                                                                              |
| ----------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| Obtener todos los artículos para el depósito 2 y saldo stock mayor a 50 | https://tiendas.axoft.com/api/Aperture/DataBy/ArtPorSaldoStock?pageSize=500&pageNumber=1 (con el JSON del ejemplo previo enviado en el body del POST) |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "SKUCode": "0200200298",
            "Description": "EJE TRANSMISOR LAVARROPAS",
            "AdditionalDescription": "",
            "AlternativeCode": "",
            "BarCode": "",
            "Commission": 0.0000000,
            "Discount": 0.0000000,
            "MeasureUnitCode": "UNI",
            "SalesMeasureUnitCode": "UNI",
            "SalesEquivalence": 1,
            "MaximumStock": 500.0000000,
            "MinimumStock": 20.0000000,
            "RestockPoint": 50.0000000,
            "Observations": "",
            "Kit": false,
            "KitValidityDateSince": null,
            "KitValidityDateUntil": null,
            "UseScale": "N",
            "Scale1": "",
            "Scale2": "",
            "BaseArticle": "",
            "ScaleValue1": "",
            "ScaleValue2": "",
            "DescriptionScale1": "",
            "DescriptionScale2": "",
            "DescriptionValueScale1": "",
            "DescriptionValueScale2": "",
            "Disabled": false,
            "ProductComposition": [],
            "ProductComments": []
        }
    ],
    "PagingError": null
}
```

<a name="clientes"></a>

#### Clientes

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de clientes, con sus direcciones de entrega y comentarios.

| **Recurso**                                                                                                                                      |
| ------------------------------------------------------------------------------------------------------------------------------------------------ |
| https://tiendas.axoft.com/api/Aperture/Customer?{pageSize}&{pageNumber}&{updatedDate}&{documentType}&{documentNumber}&{ivaCategoryCode}&[filter] |

Ejemplos

| **Para**                                                                                                                                                     | **GET**                                                                                                                |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------ | ---------------------------------------------------------------------------------------------------------------------- |
| Obtener los clientes cuyo código contenga la cadena "CL"                                                                                                     | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1&filter=CL                                    |
| Obtener los clientes con última actualización igual o posterior al 01/01/2020 a las 00:00:00 horas (UTC)                                                     | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1&updatedDate=1900-01-01T00:00:00              |
| Obtener el cliente con el tipo de documento “80” (CUIT) y el número de documento “11-11.111.111-1”. Ver Tablas de Referencia, [Tipo de Documento](#tipodoc). | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1&documentType=80&documentNumber=11-11111111-1 |
| Obtener el cliente con el tipo de documento “96” (DNI) y el número de documento “11.111.111”. Ver Tablas de Referencia, [Tipo de Documento](#tipodoc).       | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1&documentType=96&documentNumber=11111111      |
| Obtener los clientes con el tipo de categoría de IVA “RI” (Responsable inscripto). Ver Tablas de Referencia, [Condición Fiscal](#cfiscal).                   | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1&ivaCategoryCode=RI                           |
| Obtener todos los clientes                                                                                                                                   | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1                                              |

Respuesta

_Recuerde_:El código de provincia informado corresponde con la tabla de provincias proporcionada por AFIP. ([Tablas de Referencia](#tablas))

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "Code": "0500",
            "BusinessName": "0500",
            "TradeName": "",
            "Address": "",
            "PostalCode": "",
            "City": "",
            "ProvinceCode": "0",
            "TradeAddress": "",
            "PhoneNumbers": "4962-1209/2250",
            "Email": "",
            "MobilePhoneNumber": "",
            "WebPage": "",
            "IvaCategoryCode": "RI",
            "DocumentType": "96",
            "DocumentNumber": "11.111.111",
            "PriceListNumber": 1,
            "Discount": 15,
            "Observations": "",
            "DisabledDate": null,
            "SellerCode": "1",
            "SellerCode": "1",
            "SellerCode": "1",
            "CreditQuota": 15000.00,
            "LocalAccountBalance": 6500.50,
            "ForeignAccountBalance": 30.00,
            "ForeignCurrencyClause": false,
            "CreditQuotaCurrencyCode": "1",
            "UpdateDatetime": null,
            "LastUpdateUtc": "1900-01-01T00:00:00",
            "ShippingAddresses": [
                {
                    "Code": "PRINCIPAL",
                    "Address": "SARMIENTO",
                    "ProvinceCode": "0",
                    "City": "Capital Federal",
                    "PostalCode": "1407",
                    "PhoneNumber1": "4962-1209/2250",
                    "PhoneNumber2": "",
                    "DefaultAddress": "S",
                    "Enabled": "S",
                    "DeliveryHours": "",
                    "DeliversMonday": "N",
                    "DeliversTuesday": "N",
                    "DeliversWednesday": "S",
                    "DeliversThursday": "N",
                    "DeliversFriday": "S",
                    "DeliversSaturday": "N",
                    "DeliversSunday": "N"
                }
            ],
            "CustomerComments": [],
            "SellerCode": "4",
            "CreditQuota": 99999999,
            "LocalAccountBalance": 27214.99,
            "ForeignAccountBalance": 6322.45,
            "ForeignCurrencyClause": false,
            "CreditQuotaCurrencyCode": "Corriente",
            "SaleConditionCode": 2,
            "TransportCode": ""
        },
        {
            "Code": "2255",
            "BusinessName": "CLIENTE 2255 RIVADAVIA",
            "TradeName": "Distribuidora Lombardi",
            "Address": "Av. Rivadavia 6250",
            "PostalCode": "1407",
            "City": "Capital Federal",
            "ProvinceCode": "0",
            "TradeAddress": "Av. Rivadavia 5620",
            "PhoneNumbers": "4962-1209/2250",
            "Email": "Lombardi@acesnet.com.ar",
            "MobilePhoneNumber": "",
            "WebPage": "www.distribuidoraloombardi.com",
            "IvaCategoryCode": "RI",
            "DocumentType": "7",
            "DocumentNumber": "",
            "PriceListNumber": 1,
            "Discount": 10,
            "Observations": "sin observaciones",
            "DisabledDate": null,
            "SellerCode": "1",
            "CreditQuota": 40000.00,
            "LocalAccountBalance": 12000.00,
            "ForeignAccountBalance": 0.00,
            "ForeignCurrencyClause": false,
            "CreditQuotaCurrencyCode": "1",
            "ShippingAddresses": [
                {
                    "Code": "LOCAL",
                    "Address": "Av. Rivadavia 6250",
                    "ProvinceCode": "0",
                    "City": "Capital Federal",
                    "PostalCode": "1407",
                    "PhoneNumber1": "4962-1209/2250",
                    "PhoneNumber2": "",
                    "DefaultAddress": "S",
                    "Enabled": "S",
                    "DeliveryHours": "",
                    "DeliversMonday": "N",
                    "DeliversTuesday": "N",
                    "DeliversWednesday": "S",
                    "DeliversThursday": "N",
                    "DeliversFriday": "S",
                    "DeliversSaturday": "N",
                    "DeliversSunday": "N"
                },
                {
                    "Code": "LOCAL MORENO",
                    "Address": "Cabrera 4902",
                    "ProvinceCode": "1",
                    "City": "Merlo",
                    "PostalCode": "1789",
                    "PhoneNumber1": "5595-6985",
                    "PhoneNumber2": "5595-6986",
                    "DefaultAddress": "N",
                    "Enabled": "S",
                    "DeliveryHours": "",
                    "DeliversMonday": "S",
                    "DeliversTuesday": "N",
                    "DeliversWednesday": "N",
                    "DeliversThursday": "S",
                    "DeliversFriday": "N",
                    "DeliversSaturday": "N",
                    "DeliversSunday": "N"
                }
            ],
            "CustomerComments": [
                {
                    "Line": 1,
                    "Text": "sin comentarios"
                }
            ],
            "SellerCode": "2",
            "CreditQuota": 99999999,
            "LocalAccountBalance": 33578.56,
            "ForeignAccountBalance": 7572.51,
            "ForeignCurrencyClause": false,
            "CreditQuotaCurrencyCode": "Corriente",
            "SaleConditionCode": 3,
            "TransportCode": "02"
        },
        {
            "Code": "9898",
            "BusinessName": "CLIENTE 9898",
            "TradeName": "Distribuidora Lombardi",
            "Address": "Av. Rivadavia 6250",
            "PostalCode": "1407",
            "City": "Capital Federal",
            "ProvinceCode": "0",
            "TradeAddress": "TALCAHUANO 855",
            "PhoneNumbers": "4962-1209/2250",
            "Email": "Lombardi@acesnet.com.ar",
            "MobilePhoneNumber": "",
            "WebPage": "www.distribuidoraloombardi.com",
            "IvaCategoryCode": "RI",
            "DocumentType": "5",
            "DocumentNumber": "",
            "PriceListNumber": 1,
            "Discount": 10,
            "Observations": "",
            "DisabledDate": "2019-02-02T00:00:00",
            "SellerCode": "2",
            "CreditQuota": 0.00,
            "LocalAccountBalance": 0.00,
            "ForeignAccountBalance": 0.00,
            "ForeignCurrencyClause": false,
            "CreditQuotaCurrencyCode": "1",
            "ShippingAddresses": [
                {
                    "Code": "SUCU_100",
                    "Address": "SARMIENTO",
                    "ProvinceCode": "17",
                    "City": "CAPITAL",
                    "PostalCode": "1000",
                    "PhoneNumber1": "TEL",
                    "PhoneNumber2": "FAX",
                    "DefaultAddress": "S",
                    "Enabled": "S",
                    "DeliveryHours": "HORARIO DE ENTREGA ",
                    "DeliversMonday": "S",
                    "DeliversTuesday": "N",
                    "DeliversWednesday": "N",
                    "DeliversThursday": "N",
                    "DeliversFriday": "N",
                    "DeliversSaturday": "N",
                    "DeliversSunday": "N"
                }
            ],
            "CustomerComments": [],
            "SellerCode": "1",
            "CreditQuota": 99999999,
            "LocalAccountBalance": 24166.01,
            "ForeignAccountBalance": 5505.49,
            "ForeignCurrencyClause": false,
            "CreditQuotaCurrencyCode": "Corriente",
            "SaleConditionCode": 2,
            "TransportCode": "01"
        }
    ]
}
```

<a name="listaprecios"></a>

#### Listas de precios

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de listas de precios.

| **Recurso**                                                                       |
| --------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/PriceList?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                                                                                                | **GET**                                                                                                   |
| ----------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------- |
| Obtener la lista de precios cuyo número de lista es 1                                                                   | https://tiendas.axoft.com/api/Aperture/PriceList?pageSize=500&pageNumber=1&filter=1                       |
| Obtener todas las listas de precios con última actualización igual o posterior al 01/01/2020 a las 00:00:00 horas (UTC) | https://tiendas.axoft.com/api/Aperture/PriceList?pageSize=500&pageNumber=1&lastUpdate=2020-01-01T00:00:00 |
| Obtener todas las listas de precios                                                                                     | https://tiendas.axoft.com/api/Aperture/PriceList?pageSize=500&pageNumber=1                                |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "PriceListNumber": 1,
            "Description": "Venta Mayorista1",
            "CommonCurrency": true,
            "IvaIncluded": true,
            "InternalTaxIncluded": true,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00",
            "Disabled": false
        },
        {
            "PriceListNumber": 2,
            "Description": "Venta Minorista1",
            "CommonCurrency": true,
            "IvaIncluded": true,
            "InternalTaxIncluded": true,
            "ValidityDateSince": null,
            "ValidityDateUntil": null,
            "Disabled": false
        },
        {
            "PriceListNumber": 4,
            "Description": "LISTA DE PRECIOS1",
            "CommonCurrency": false,
            "IvaIncluded": true,
            "InternalTaxIncluded": true,
            "ValidityDateSince": null,
            "ValidityDateUntil": null,
            "Disabled": false
        },
        {
            "PriceListNumber": 20,
            "Description": "LISTA VENTA MAYORIST",
            "CommonCurrency": true,
            "IvaIncluded": true,
            "InternalTaxIncluded": true,
            "ValidityDateSince": "2019-01-01T00:00:00",
            "ValidityDateUntil": "2019-02-01T00:00:00",
            "Disabled": false
        }
    ]
}
```

<a name="precios"></a>

#### Precios

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de precios.

Solo se mostrarán precios de:

- Artículos que en **Tango Gestión y Tango Punto de Venta Argentina** cumplan:
  - Perfil de Venta, Compra-Venta o Inhabilitado.
  - Tipo Simple, Fórmula, o Kit fijo.
  - No sean artículos Base.
  - No estén inhabilitados.
- Listas de precios que en **Tango Gestión y Tango Punto de Venta Argentina** no estén inhabilitadas.

| **Recurso**                                                                             |
| --------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Price?{pageSize}&{pageNumber}&[filter]&[SKUCode] |

Ejemplos

| **Para**                                                                                                                                                       | **GET**                                                                                               |
| -------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| Obtener los precios de la lista de precios cuyo número de lista es 1                                                                                           | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1&filter=1                       |
| Obtener los precios de la lista de precios cuyo número de lista es 1 y el código de artículo contenga la cadena "01"                                           | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1&filter=1&SKUCode=01            |
| Obtener todos los precios cuya última sincronización a Tiendas sea igual o posterior al 01/01/2020 a las 00:00:00 horas (UTC)                                                  | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1&lastUpdate=2020-01-01T00:00:00 |
| Obtener todos los precios cuya fecha de modificación en Tango Gestión sea igual o posterior al 01/01/2020 a las 00:00:00 horas                                                  | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1&datePrice=2020-01-01T00:00:00 |
| Obtener todos los precios                                                                                                                                      | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1                                |
| Obtener el precio de un artículo cuyo identificador del registro sea igual a 1. El identificador se recibe a traves de las [notificaciones](#notificaciones).  | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1&id=1                           |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": true
    },
    "Data": [
        {
            "PriceListNumber": 1,
            "SKUCode": "0200200065",
            "Price": 45.08,
            "DatePrice": "2018-01-01T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100100134",
            "Price": 20215.55,
            "DatePrice": "2018-01-01T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100100265",
            "Price": 30330.55,
            "DatePrice": "2018-01-02T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100200528",
            "Price": 87408.05,
            "DatePrice": "2018-01-02T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100200530",
            "Price": 53465,
            "DatePrice": "2018-01-03T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100200630",
            "Price": 41168.05,
            "DatePrice": "2018-01-03T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200100013",
            "Price": 27.51,
            "DatePrice": "2018-01-03T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200100124",
            "Price": 98.26,
            "DatePrice": "2018-01-03T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200200033",
            "Price": 41.62,
            "DatePrice": "2018-01-04T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200200034",
            "Price": 199.46,
            "DatePrice": "2018-01-04T00:00:00",
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        }
    ]
}
```

<a name="precioscliente"></a>

#### Precios por cliente

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de precios por cliente.

Solo se mostrarán precios por cliente de:

- Artículos que en **Tango Gestión y Tango Punto de Venta Argentina** cumplan:
  - Perfil de Venta, Compra-Venta o Inhabilitado.
  - Tipo Simple, Fórmula, o Kit fijo.
  - No sean artículos Base.
  - No estén inhabilitados.
- Listas de precios que en **Tango Gestión y Tango Punto de Venta Argentina** no estén inhabilitadas.
- Clientes que en **Tango Gestión y Tango Punto de Venta Argentina** no estén inhabilitados.

| **Recurso**                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/PriceByCustomer?{pageSize}&{pageNumber}&[filter]&[SKUCode]&[PriceListNumber] |

Ejemplos

| **Para**                                                                                                                                                            | **GET**                                                                                                                      |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| Obtener los precios de los clientes cuyo código contenga la cadena "CL00028"                                                                                        | https://tiendas.axoft.com/api/Aperture/PriceByCustomer?pageSize=500&pageNumber=1&filter=CL00028                              |
| Obtener los precios de los clientes cuyo código contenga la cadena "CL00028", el código de artículo contenga la cadena "01" y la lista de precios la lista número 2 | https://tiendas.axoft.com/api/Aperture/PriceByCustomer?pageSize=500&pageNumber=1&filter=CL00028&SKUCode=01&PriceListNumber=2 |
| Obtener todos los precios por cliente con última actualización igual o posterior al 01/01/2020 a las 00:00:00 horas (UTC)                                           | https://tiendas.axoft.com/api/Aperture/PriceByCustomer?pageSize=500&pageNumber=1&lastUpdate=2020-01-01T00:00:00              |
| Obtener todos los precios por cliente                                                                                                                               | https://tiendas.axoft.com/api/Aperture/PriceByCustomer?pageSize=500&pageNumber=1                                             |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "SKUCode": "0100100129",
            "CustomerCode": "010001",
            "Price": 10,
            "PriceListNumber": 1
        },
        {
            "SKUCode": "0100100129",
            "CustomerCode": "010002",
            "Price": 0,
            "PriceListNumber": 1
        },
        {
            "SKUCode": "0100100129",
            "CustomerCode": "020025",
            "Price": 20,
            "PriceListNumber": 1
        },
        {
            "SKUCode": "0100100129",
            "CustomerCode": "CLAUS",
            "Price": 30,
            "PriceListNumber": 1
        },
        {
            "SKUCode": "0100100129",
            "CustomerCode": "EXTER",
            "Price": 0,
            "PriceListNumber": 1
        }
    ]
}

```

<a name="descuentoscliente"></a>

#### Descuentos por cliente

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de descuentos por cliente.

Solo se mostrarán descuentos por cliente de:

- Artículos que en **Tango Gestión y Tango Punto de Venta Argentina** cumplan:
  - Perfil de Venta, Compra-Venta o Inhabilitado.
  - Tipo Simple, Fórmula, o Kit fijo.
  - No sean artículos Base.
  - No estén inhabilitados.
- Clientes que en **Tango Gestión y Tango Punto de Venta Argentina** no estén inhabilitados.

| **Recurso**                                                                                          |
| ---------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/DiscountByCustomer?{pageSize}&{pageNumber}&[filter]&[SKUCode] |

Ejemplos

| **Para**                                                                                                                                    | **GET**                                                                                                       |
| ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| Obtener los descuentos por cliente de los clientes cuyo código contenga la cadena "CL00028"                                                 | https://tiendas.axoft.com/api/Aperture/DiscountByCustomer?pageSize=500&pageNumber=1&filter=CL00028            |
| Obtener los descuentos por cliente de los clientes cuyo código contenga la cadena "CL00028" y el código de artículo contenga la cadena "01" | https://tiendas.axoft.com/api/Aperture/DiscountByCustomer?pageSize=500&pageNumber=1&filter=CL00028&SKUCode=01 |
| En el caso que se desee realizar una búsqueda exacta del dato indicado a través de los parámetros mencionados se deberá agregar "UseEqual=TRUE"| https://tiendas.axoft.com/api/Aperture/DiscountByCustomer?pageSize=500&pageNumber=1&filter=CL00028&SKUCode=01&UseEqual=True                            |
| Obtener todos los descuentos por cliente                                                                                                    | https://tiendas.axoft.com/api/Aperture/DiscountByCustomer?pageSize=500&pageNumber=1                           |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": true
    },
    "Data": [
        {
            "SKUCode": "0100200703",
            "CustomerCode": "010002",
            "Discount": 0
        },
        {
            "SKUCode": "0100100134",
            "CustomerCode": "99998",
            "Discount": 15
        },
        {
            "SKUCode": "0100100129",
            "CustomerCode": "CLI_5",
            "Discount": 2
        },
        {
            "SKUCode": "0100100134",
            "CustomerCode": "CLI_5",
            "Discount": 2
        },
        {
            "SKUCode": "0100100135",
            "CustomerCode": "CLI_5",
            "Discount": 2
        },
        {
            "SKUCode": "0100100136",
            "CustomerCode": "CLI_5",
            "Discount": 2
        },
        {
            "SKUCode": "0100100150",
            "CustomerCode": "CLI_5",
            "Discount": 2
        },
        {
            "SKUCode": "0100100151",
            "CustomerCode": "CLI_5",
            "Discount": 2
        },
        {
            "SKUCode": "0100100152",
            "CustomerCode": "CLI_5",
            "Discount": 2
        },
        {
            "SKUCode": "0100100153",
            "CustomerCode": "CLI_5",
            "Discount": 2
        }
    ]
}
```

<a name="saldosstock"></a>

#### Saldos de stock

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de saldos de stock detallados por sucursal, depósito y artículo; o acumulado por artículo.
En el campo **Quantity** se muestra la cantidad física en stock 1.
En el campo **EngagedQuantity** se muestra la cantidad comprometida del stock 1.
En el campo **PendingQuantity** se muestra la cantidad pendiente de ingresar al stock 1.
En el campo **SecondQuantity** se muestra la cantidad física en stock 2.
En el campo **SecondEngagedQuantity** se muestra la cantidad comprometida del stock 2.
En el campo **SecondPendingQuantity** se muestra la cantidad pendiente de ingresar al stock 2.

Solo se mostrarán saldos de stock de:

- Artículos que en **Tango Gestión y Tango Punto de Venta Argentina** cumplan:
  - Perfil de Venta, Compra-Venta o Inhabilitado.
  - Tipo Simple, Fórmula, o Kit fijo.
  - No sean artículos Base.
  - No estén inhabilitados.
- Depósitos que en **Tango Gestión y Tango Punto de Venta Argentina** no estén inhabilitados.

| **Recurso**                                                                                                                                                       |
| ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Stock?{pageSize}&{pageNumber}&[filter]&[groupByProduct]&[discountPendingOrders]&[storeNumber]&[warehouseCode]&[lastUpdate] |

Ejemplos

| **Para**                                                                                                                                                                                                                                        | **GET**                                                                                                        |
| ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| Obtener los saldos de stock de los artículos cuyo código contenga la cadena "01"                                                                                                                                                                | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&filter=01                               |
| Obtener los saldos de stock de los artículos cuyo código contenga la cadena "01", la sucursal sea 1 y el depósito corresponda al código 2. En este caso no es válido agregar la agrupación por producto (groupByProduct=true)                   | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&filter=01&StoreNumber=1&WarehouseCode=2 |
| Obtener los saldos de stock acumulados por artículo (En este caso la consulta no devolverá datos en los campos "StoreNumber" y "WarehouseCode")                                                                                                 | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&groupByProduct=true                     |
| Obtener los saldos de stock acumulados por artículo indicando que depósitos y sucursales se desea consultar indicando dichos valores separados por coma (,). En el ejemplo se muestra el caso donde existe un código de depósito que posee un espacio en blanco " 1" represenado como "_1". En el caso de no informar los parámetros se considerarán todos los depósitos y sucursales  | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&groupByProduct=true&WarehouseCode=01,_1,02&StoreNumber=5,6                     |
| Obtener los saldos de stock, restando al mismo las órdenes pendientes de revisión (en el caso de no solicitar agrupado por artículo, los registros de la cantidad en órdenes no devolverán datos en los campos "StoreNumber" y "WarehouseCode") | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&discountPendingOrders=true              |
| Obtener todos los saldos de stock con última actualización igual o posterior al 01/01/2020 a las 00:00:00 horas (UTC)                                                                                                                           | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&lastUpdate=2020-01-01T00:00:00          |
| En el caso que se desee realizar una búsqueda exacta del dato indicado a través de los parámetros mencionados se deberá agregar "UseEqual=TRUE"                                                                                                 | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&WarehouseCode=2&UseEqual=true           |
| Obtener todos los saldos de stock                                                                                                                                                                                                               | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1                                         |
| Obtener el saldo de un artículo cuyo identificador del registro sea igual a 1. El identificador se recibe a traves de las [notificaciones](#notificaciones).                                                                                          | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&id=1                                    |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": true
    },
    "Data": [
        {
            "StoreNumber": 1,
            "WarehouseCode": "1",
            "SKUCode": "0100100129",
            "Quantity": 244,
            "EngagedQuantity": 5.00,
            "PendingQuantity": 2.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "2",
            "SKUCode": "0100100129",
            "Quantity": 5,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 1.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "2",
            "SKUCode": "0100100134",
            "Quantity": 3,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "1",
            "SKUCode": "0100100134",
            "Quantity": 115,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "1",
            "SKUCode": "0100100135",
            "Quantity": 102,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "80",
            "SKUCode": "0100100136",
            "Quantity": 10,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "1",
            "SKUCode": "0100100136",
            "Quantity": 95,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "1",
            "SKUCode": "0100100150",
            "Quantity": 115,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "1",
            "SKUCode": "0100100151",
            "Quantity": 100,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        },
        {
            "StoreNumber": 1,
            "WarehouseCode": "1",
            "SKUCode": "0100100152",
            "Quantity": 182,
            "EngagedQuantity": 0.00,
            "PendingQuantity": 0.00
        }
    ]
}
```

<div id="vendedores" />

#### Vendedores

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de vendedores.

| **Recurso**                                                                      |
| -------------------------------------------------------------------------------- |
| <https://tiendas.axoft.com/api/Aperture/Seller?{pageSize}&{pageNumber}&[filter]> |

Ejemplos

| **Para**                                         | **GET**                                                                          |
| ------------------------------------------------ | -------------------------------------------------------------------------------- |
| Obtener el vendedor cuyo código de vendedor es 4 | https://tiendas.axoft.com/api/Aperture/Seller?pageSize=500&pageNumber=1&filter=4 |
| Obtener todos los vendedores                     | https://tiendas.axoft.com/api/Aperture/Seller?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "SellerCode": "1",
            "Name": "Carlos Perez",
            "CommissionPercentage": 5.00,
            "Disabled": false
        },
        {
            "SellerCode": "2",
            "Name": "Javier Garcia",
            "CommissionPercentage": 6.00,
            "Disabled": false
        }
    ]
}
```

<div id="monedas" />

#### Monedas

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de monedas.

| **Recurso**                                                                        |
| ---------------------------------------------------------------------------------- |
| <https://tiendas.axoft.com/api/Aperture/Currency?{pageSize}&{pageNumber}&[filter]> |

Ejemplos

| **Para**                                     | **GET**                                                                            |
| -------------------------------------------- | ---------------------------------------------------------------------------------- |
| Obtener la moneda cuyo código de moneda es 2 | https://tiendas.axoft.com/api/Aperture/Currency?pageSize=500&pageNumber=1&filter=2 |
| Obtener todas las monedas                    | https://tiendas.axoft.com/api/Aperture/Currency?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "CurrencyCode": "1",
            "Description": "Pesos",
            "Symbol": "$",
            "Type": "Corriente",
            "RG1547Code": "80",
            "AFIPCode": "PES"
        },
        {
            "CurrencyCode": "2",
            "Description": "Dolares",
            "Symbol": "u$s",
            "Type": "Extranjera contable",
            "RG1547Code": "",
            "AFIPCode": "DOL"
        }
    ]
}
```

<div id="transportes" />

#### Transportes

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener los datos de transportistas.

| **Recurso**                                                                |
| -------------------------------------------------------------------------- |
| <https://tiendas.axoft.com/api/Aperture/Transport?{pageSize}&{pageNumber}> |

Ejemplos

| **Para**                                | **GET**                                                                              |
| --------------------------------------- | ------------------------------------------------------------------------------------ |
| Obtener el transporte cuyo código es 02 | https://tiendas.axoft.com/api/Aperture/Transport?pageSize=500&pageNumber=1&filter=02 |
| Obtener todos los transportes           | https://tiendas.axoft.com/api/Aperture/Transport?pageSize=500&pageNumber=1           |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 10,
        "MoreData": false
    },
    "Data": [
        {
            "Code": "01",
            "Name": "TRANSPORTE PROPIO",
            "IVACategory": "Responsable Inscripto",
            "Cuit": null,
            "SurchargePercentage": 10.0000000,
            "Address": "Av.Santa Fe 1284 ",
            "PostalCode": "1010",
            "City": "Capital Federal",
            "ProvinceCode": "01",
            "PhoneNumbers": "4816-2589",
            "Email": "info@ejemplo.com.ar",
            "WebPage": "www.ejemplo.com.ar",
            "Comments": null
        },
        {
            "Code": "02",
            "Name": "TRANSPORTES LA ESTRELLA",
            "IVACategory": "Responsable Inscripto",
            "Cuit": null,
            "SurchargePercentage": 0.0000000,
            "Address": "Salta 348 ",
            "PostalCode": "1010",
            "City": "Capital Federal",
            "ProvinceCode": "01",
            "PhoneNumbers": "4816-2695",
            "Email": "transporteestrella@ejemplo.com.ar",
            "WebPage": "www.transporteestrella.com.ar",
            "Comments": null
        },
        {
            "Code": "03",
            "Name": "TRANSPORTE SILVANA",
            "IVACategory": "",
            "Cuit": null,
            "SurchargePercentage": null,
            "Address": "",
            "PostalCode": "",
            "City": "",
            "ProvinceCode": "01",
            "PhoneNumbers": "",
            "Email": "",
            "WebPage": "",
            "Comments": null
        }
    ],
    "PagingError": null
}
```

<div id="condicionesventa" />

#### Condiciones de venta

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener los datos de las condiciones de venta disponibles.

| **Recurso**                                                                    |
| ------------------------------------------------------------------------------ |
| <https://tiendas.axoft.com/api/Aperture/SaleCondition?{pageSize}&{pageNumber}> |

Ejemplos

| **Para**                                       | **GET**                                                                                 |
| ---------------------------------------------- | --------------------------------------------------------------------------------------- |
| Obtener la condición de venta cuyo código es 3 | https://tiendas.axoft.com/api/Aperture/SaleCondition?pageSize=500&pageNumber=1&filter=3 |
| Obtener todas las condiciones de venta         | https://tiendas.axoft.com/api/Aperture/SaleCondition?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 10,
        "MoreData": false
    },
    "Data": [
        {
            "Code": 1,
            "Description": "CONTADO",
            "Cash": true,
            "GenerateAlternativeDate": false,
            "GenerateDebitLatePayment": false
        },
        {
            "Code": 2,
            "Description": "30 / 60 DIAS",
            "Cash": false,
            "GenerateAlternativeDate": false,
            "GenerateDebitLatePayment": true
        },
        {
            "Code": 3,
            "Description": "30/60/90 CON INTERES",
            "Cash": false,
            "GenerateAlternativeDate": false,
            "GenerateDebitLatePayment": false
        },
        {
            "Code": 4,
            "Description": "DESCUENTO POR PRONTO PAGO",
            "Cash": false,
            "GenerateAlternativeDate": true,
            "GenerateDebitLatePayment": true
        },
        {
            "Code": 5,
            "Description": "A 30 DIAS CON RECARGO",
            "Cash": false,
            "GenerateAlternativeDate": true,
            "GenerateDebitLatePayment": true
        },
        {
            "Code": 6,
            "Description": "CONDICIÓN PARA ABONOS MENSUALES",
            "Cash": false,
            "GenerateAlternativeDate": false,
            "GenerateDebitLatePayment": true
        },
        {
            "Code": 7,
            "Description": "CONTADO CON RECIBO",
            "Cash": false,
            "GenerateAlternativeDate": false,
            "GenerateDebitLatePayment": false
        },
        {
            "Code": 10,
            "Description": "Cuenta corriente ",
            "Cash": false,
            "GenerateAlternativeDate": false,
            "GenerateDebitLatePayment": true
        },
        {
            "Code": 20,
            "Description": "FCE TRUE",
            "Cash": false,
            "GenerateAlternativeDate": false,
            "GenerateDebitLatePayment": false
        }
    ],
    "PagingError": null
}
```

<a name="ClasificadorArticulos"></a>

### Clasificador de artículos

<a name="CarpetasClasificadorArticulos"></a>

#### Carpetas de artículos

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de carpetas clasificador de artículos.

| **Recurso**                                                                                      |
| ------------------------------------------------------------------------------------------------ |
| https://tiendas.axoft.com/api/Aperture/ProductsFolderClassifier?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                            | **GET**                                                                                            |
| ----------------------------------- | -------------------------------------------------------------------------------------------------- |
| Obtener la carpeta cuyo número es 2 | https://tiendas.axoft.com/api/Aperture/ProductsFolderClassifier?pageSize=500&pageNumber=1&filter=2 |
| Obtener todas las carpetas          | https://tiendas.axoft.com/api/Aperture/ProductsFolderClassifier?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "IdFolder": 2,
            "Name": "Rubro",
            "IdParent": 1
        }
    ],
    "PagingError": null
}

```

<a name="ArticulosEnCarpetaClasificador"></a>

#### Artículos en las carpetas

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de artículos en carpetas por clasificador.

| **Recurso**                                                                            |
| -------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/ProductsFolder?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                          | **GET**                                                                                  |
| ------------------------------------------------- | ---------------------------------------------------------------------------------------- |
| Obtener los artículos cuyo número de carpeta es 3 | https://tiendas.axoft.com/api/Aperture/ProductsFolder?pageSize=500&pageNumber=1&filter=3 |
| Obtener todas los artículos                       | https://tiendas.axoft.com/api/Aperture/ProductsFolder?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 50,
        "MoreData": false
    },
    "Data": [
        {
            "SkuCode": "0100100265",
            "IdFolder": 3
        },
        {
            "SkuCode": "0100200703",
            "IdFolder": 3
        },
        {
            "SkuCode": "0100200708",
            "IdFolder": 3
        },
        {
            "SkuCode": "030010001",
            "IdFolder": 3
        },
        {
            "SkuCode": "0100100266",
            "IdFolder": 3
        },
        {
            "SkuCode": "0100100267",
            "IdFolder": 3
        }
    ],
    "PagingError": null
}
```

<a name="ArticulosRelacionadosClasificador"></a>

#### Relaciones

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener las relaciones de las carpertas de artículos clasificador.

| **Recurso**                                                                              |
| ---------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/ProductsRelation?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                                            | **GET**                                                                                    |
| ------------------------------------------------------------------- | ------------------------------------------------------------------------------------------ |
| Obtener las relaciones de los artículos cuyo número de carpeta es 3 | https://tiendas.axoft.com/api/Aperture/ProductsRelation?pageSize=500&pageNumber=1&filter=3 |
| Obtener todas los artículos                                         | https://tiendas.axoft.com/api/Aperture/ProductsRelation?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 50,
        "MoreData": false
    },
    "Data": [
        {
            "SkuCode": "0100200708",
            "IdFolder": 3,
            "RelationName": "Sustitutos",
            "ShowRelation": true
        },
        {
            "SkuCode": "0100100265",
            "IdFolder": 3,
            "RelationName": "Sustitutos",
            "ShowRelation": true
        },
        {
            "SkuCode": "0100200703",
            "IdFolder": 3,
            "RelationName": "Sustitutos",
            "ShowRelation": true
        }
    ],
    "PagingError": null
}
```

<a name="ClasificadorClientes"></a>

### Clasificador de clientes

<a name="CarpetasClasificadorClientes"></a>

#### Carpetas de clientes

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de carpetas clasificador de clientes.

| **Recurso**                                                                                       |
| ------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/CustomersFolderClassifier?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                            | **GET**                                                                                             |
| ----------------------------------- | --------------------------------------------------------------------------------------------------- |
| Obtener la carpeta cuyo número es 2 | https://tiendas.axoft.com/api/Aperture/CustomersFolderClassifier?pageSize=500&pageNumber=1&filter=2 |
| Obtener todas las carpetas          | https://tiendas.axoft.com/api/Aperture/CustomersFolderClassifier?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 500,
        "MoreData": false
    },
    "Data": [
        {
            "IdFolder": 2,
            "Name": "Segmento de mercado",
            "IdParent": 1
        }
    ],
    "PagingError": null
}
```

<a name="ClientesEnCarpetaClasificador"></a>

#### Clientes en las carpetas

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de clientes en carpetas por clasificador.

| **Recurso**                                                                             |
| --------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/CustomersFolder?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                         | **GET**                                                                                   |
| ------------------------------------------------ | ----------------------------------------------------------------------------------------- |
| Obtener los clientes cuyo número de carpeta es 3 | https://tiendas.axoft.com/api/Aperture/CustomersFolder?pageSize=500&pageNumber=1&filter=3 |
| Obtener todas los artículos                      | https://tiendas.axoft.com/api/Aperture/CustomersFolder?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 50,
        "MoreData": false
    },
    "Data": [
        {
            "CustomerCode": "010001",
            "IdFolder": 3
        }
    ],
    "PagingError": null
}

```

<a name="ClientesRelacionadosClasificador"></a>

#### Relaciones

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener las relaciones de las carpertas de clientes clasificador.

| **Recurso**                                                                               |
| ----------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/CustomersRelation?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                                | **GET**                                                                                     |
| ------------------------------------------------------- | ------------------------------------------------------------------------------------------- |
| Obtener las relaciones de las carpetas cuyo número es 3 | https://tiendas.axoft.com/api/Aperture/CustomersRelation?pageSize=500&pageNumber=1&filter=3 |
| Obtener todas los artículos                             | https://tiendas.axoft.com/api/Aperture/CustomersRelation?pageSize=500&pageNumber=1          |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 50,
        "MoreData": false
    },
    "Data": [
        {
            "CustomerCode": "010001",
            "IdFolder": 3,
            "RelationName": "Alternativos",
            "ShowRelation": false
        }
    ],
    "PagingError": null
}

```

<a name="CotizacionMonedaExtranjera"></a>

### Cotización moneda extranjera

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener el valor de la cotización de la moneda extranjera contable.

| **Recurso**                                                              |
| ------------------------------------------------------------------------ |
| https://tiendas.axoft.com/api/Aperture/CurrencyExchangeRate?{lastUpdate} |

Ejemplo

| **Para**                                               | **GET**                                                                                    |
| ------------------------------------------------------ | ------------------------------------------------------------------------------------------ |
| Obtener el valor de la cotización                      | https://tiendas.axoft.com/api/Aperture/CurrencyExchangeRate                                |
| Valor de la cotización, actualizada después de X fecha | https://tiendas.axoft.com/api/Aperture/CurrencyExchangeRate?lastUpdate=2020-01-01T00:00:00 |

Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 50,
        "MoreData": false
    },
    "Data": [
        {
            "Value": 38.0000000
        }
    ],
    "PagingError": null
}
```

<a name="Publicaciones"></a>

### Publicaciones

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener las relaciones entre las publicaciones del eCommerce y el código de artículo en Tango asociado.

Aclaración: Sólo se mostrarán relaciones que se hayan sincronizado previamente al procesar órdenes que contengan el identificador de la publicación.

| **Recurso**                                                                                                       |
| ----------------------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Publications?{pageSize}&{pageNumber}&{productCode}&{skuCode}&{variantCode} |

Ejemplos

| **Para**                                                                                                           | **GET**                                                                                                   |
| ------------------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------- |
| Obtener, partiendo del identificador de la publicación 1, el artículo de Tango asociado                            | https://tiendas.axoft.com/api/Aperture/Publications?pageSize=500&pageNumber=1&productCode=1               |
| Obtener, partiendo del identificador de la publicación 1 y el código de variación 2, el artículo de Tango asociado | https://tiendas.axoft.com/api/Aperture/Publications?pageSize=500&pageNumber=1&productCode=1&variantCode=2 |
| Obtener, partiendo del código del artículo en Tango 1, el identificador de la publicación asociada                 | https://tiendas.axoft.com/api/Aperture/Publications?pageSize=500&pageNumber=1&skuCode=1                   |
| Obtener todas las relaciones entre identificador de publicación y artículo de Tango asociado                       | https://tiendas.axoft.com/api/Aperture/Publications?pageSize=500&pageNumber=1                             |

Respuesta

```
{
  "Paging": {
    "PageNumber": 1,
    "PageSize": 50,
    "MoreData": false
  },
  "Data": [
    {
      "ProductCode": "040000000040",
      "Description": "CEMENTO AVELLANEDA",
      "VariantCode": null,
      "VariantDescription": null,
      "SkuCode": "0200100124"
    },
    {
      "ProductCode": "010040",
      "Description": "TV",
      "VariantCode": "BL",
      "VariantDescription": "TV BLANCO",
      "SkuCode": "010040001RBL"
    },
    {
      "ProductCode": "010040",
      "Description": "TV",
      "VariantCode": "NG",
      "VariantDescription": "TV NEGRO",
      "SkuCode": "010040002NG"
    }
  ],
  "PagingError": null
}
```

<a name="ComprobantesDeFacturacion"></a>

### Comprobantes de facturación

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener las relaciones entre las órdenes del eCommerce y los comprobantes electrónicos asociados al pedido facturado de esas órdenes.

Se incorpora en la respuesta la URL al archivo PDF del comprobante.

Aclaración: solo se mostrarán relaciones para el caso de comprobantes electrónicos.

| **Recurso**                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Invoices?{pageSize}&{pageNumber}&{orderId}&{orderNumber}&{fromDate}&{toDate} |

Ejemplos

| **Para**                                                                                                                                                                   | **GET**                                                                                                         |
| -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| Obtener, partiendo del identificador de la orden 1, el tipo y número de comprobante asociado                                                                               | https://tiendas.axoft.com/api/Aperture/Invoices?pageSize=500&pageNumber=1&orderId=1                             |
| Obtener, partiendo del número de la orden 1, el tipo y número de comprobante asociado                                                                                      | https://tiendas.axoft.com/api/Aperture/Invoices?pageSize=500&pageNumber=1&orderNumber=1                         |
| Obtener todas las relaciones entre orden y comprobantes de facturación, cuya fecha de la orden se encuentre entre 1/1/2020 y 31/12/2020 (no son obligatorias ambas fechas) | https://tiendas.axoft.com/api/Aperture/Invoices?pageSize=500&pageNumber=1&fromDate=2020-01-01&toDate=2020-12-31 |
| Obtener todas las relaciones entre orden y comprobantes, para órdenes del día de hoy                                                                                       | https://tiendas.axoft.com/api/Aperture/Invoices?pageSize=500&pageNumber=1                                       |

Respuesta

```
{
  "Paging": {
    "PageNumber": 1,
    "PageSize": 50,
    "MoreData": false
  },
  "Data": [
    {
      "OrderId": "25",
      "OrderNumber": "12345678",
      "OrderDate": "2020-11-05T00:00:00",
      "InvoiceType": "FAC",
      "InvoiceNumber": "A0000100000245",
      "InvoiceFileUrl": "https://nexo-tiendas-cloud-ue1-data.s3.amazonaws.com/PDF,
      "InvoiceFileExpiration": "“2020-12-17T00:00:00"
    },
    {
      "OrderId": "34",
      "OrderNumber": "56781234",
      "OrderDate": "2020-11-05T00:00:00",
      "InvoiceType": "FAC",
      "InvoiceNumber": "A0000100000200",
      "InvoiceFileUrl": "https://nexo-tiendas-cloud-ue1-data.s3.amazonaws.com/PDF",
      "InvoiceFileExpiration": "“2020-12-18T00:00:00"
    }
  ],
  "PagingError": null
}
```

### Talonarios

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener los talonarios definidos en Tango Gestión.

| **Recurso**                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Counterfoil?{pageSize}&{pageNumber}&{voucher} |

Ejemplos

| **Para**                                                                                                                                                                   | **GET**                                                                                                         |
| -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| Obtener, partiendo del identificador del talonario de pedidos                                                                               | https://tiendas.axoft.com/api/Aperture/Counterfoil?pageSize=500&pageNumber=1&voucher=PED                             |
| 
Respuesta

```
{
    "Paging": {
        "PageNumber": 1,
        "PageSize": 50,
        "MoreData": false
    },
    "Data": [
        {
            "CounterfoilCode": 6,
            "Description": "PEDIDOS",
            "CounterfoilType": "",
            "Voucher": "PED",
            "CounterfoilExpiration": "2024-11-30T00:00:00"
        }
    ],
    "PagingError": null
}
```

<a name="EstadosOrdenes"></a>

### Estado de órdenes

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener información de las órdenes, su estado y comprobante de facturación asociado.

Los distintos estados por los que puede pasar una orden y que pueden ser consultados en este recurso, son los siguientes: 

- INGRESADA: estado inicial de una orden en nube.  
- RECIBIDA: la orden fue sincronizada a tierra para generar el pedido.  
- EN PROCESO: la orden tiene un pedido generado.  
- RECHAZADA: orden rechazada desde el proceso de “Revisión de pedidos Tango Tiendas”
- CANCELADA: la orden fue cancelada en la tienda origen.
- CANCELADA APROB: se procesó la cancelación en forma automática o manualmente desde el  proceso de “Revisión de pedidos Tango Tiendas”
- ANULADA: Orden con pedido generado y posteriormente anulado sin reconstruir cantidades.  
- FINALIZADA: la orden tiene una factura generada.  
- PENDIENTE: Corresponde a las órdenes que fueron enviadas y se validaron correctamente, pero se encuentran en cola de procesamiento. 

Si se desea obtener las facturas asociadas a las órdenes consultadas, si existen, se debe enviar el parámetro IncludeInvoices con el valor TRUE. 

Se debe tener en cuenta que, la información del comprobante se podrá visualizar una vez que el mismo se encuentre disponible, es decir que haya sido enviado desde el proceso de “Administración de comprobantes electrónicos” en Tango. 

| **Recurso**                                                                                                         |
| ------------------------------------------------------------------------------------------------------------------- |
|  https://tiendas.axoft.com/api/Aperture/order?{pageSize}&{pageNumber}&[filter]                                     |

Ejemplos

| **Para**                                                                                                                                                                   | **GET**                                                                                                         |
| -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| Obtener información de las órdenes 1, 5 y 7                                                                                | https://tiendas.axoft.com/api/Aperture/order?pageSize=500&pageNumber=1&OrderId= [1,5,7]                              |
| Obtener información de las órdenes cuyo estado sea “INGRESADA” o “RECIBIDA”                                                | https://tiendas.axoft.com/api/Aperture/order?pageSize=500&pageNumber=1&State= [“INGRESADA”, “RECIBIDA”]                          |
| Obtener información de las órdenes cuya fecha de generación se encuentre entre 01/01/2022 y 01/01/2023 (no son obligatorias ambas fechas)  | https://tiendas.axoft.com/api/Aperture/order?pageSize=500&pageNumber=1&fromDate=2022-01-01&toDate=2023-01-01  |
| Obtener información de todas las ordenes generadas a partir del 01/01/2023 incluyendo los comprobantes de facturación.                     | https://tiendas.axoft.com/api/Aperture/order/?pageSize=500&pageNumber=1&fromDate=2023-01-01&IncludeInvoices=true                                        | 

Respuesta

```
{
   "Paging":{
      "PageNumber":1,
      "PageSize":500,
      "MoreData":false
   },
   "Data":[
      {
         "OrderID":"116079",
         "Date":"2023-04-14T12:08:02",
         "Status":"FINALIZADA",
         "InvoiceType":"FAC",
         "InvoiceNumber":"B0020400000013",
         "InvoiceFileUrl":"https://nexo-tiendas-test-ue1-data.s3.amazonaws.com/PDF/30867/788466/FACB0020400000013.pdf?AWSAccessKeyId=AKIA6AQ4YDSLGJLAFLWT&Expires=1682678459&Signature=ovPKkSEVXK3jAR%2B6P8F8esIYwpQ%3D",
         "InvoiceFileExpiration":"2023-04-28T07:40:58.7733276-03:00"
      },
      {
         "OrderID":"116080",
         "Date":"2023-04-14T12:08:02",
         "Status":"ANULADA",
         "InvoiceType":null,
         "InvoiceNumber":null,
         "InvoiceFileUrl":null,
         "InvoiceFileExpiration":null
      }
   ],
   "OrderError":  
   {
	"Orders": "116081, 116082, 116083",
	"Message": “Unexisting Order ID”
   }, 
   "PagingError":null
}
```
