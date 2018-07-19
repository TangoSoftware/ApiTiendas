<a name="inicio"></a>
Tango Software - API Tiendas
=======

 + [Instalación](#instalacion)
    + [Versiones soportadas de Tango Gestión](#versiones)
      + [Generalidades](#generalidades)
      + [Ambientes](#ambientes)
    + [Asociar aplicación con API](#asociarapi)
    + [Datos del Json](#djson)
      + [Tablas de Referencia](#tablas)



<a name="instalacion"></a>
## Instalación

<a name="versiones"></a>
### Versiones soportadas de Tango Gestión

<a name="generalidades"></a>
#### Generalidades
[<sub>Volver</sub>](#inicio)

Esta versión soporta órdenes de pedido únicamente en moneda nacional argentina.

Aceptando hasta 2 decimales en los datos de importes y precios.

<a name="ambientes"></a>
#### Ambientes
[<sub>Volver</sub>](#inicio)

• Ambiente de testeo

Para configurar el ambiente de testeo desde Tango Sync debe asociar una empresa de nube con una empresa ejemplo de Tango Gestión.

• Ambiente de producción

Para configurar el ambiente de testeo desde Tango Sync debe asociar una empresa de nube con una empresa operativa de Tango Gestión.


<a name="asociarapi"></a>
### Asociar aplicación con API
[<sub>Volver</sub>](#inicio)

Luego de haber vinculado una empresa de nube con una empresa de Tango Gestión, acceda a nexo Tiendas / API para obtener el un Access Token que le permitirá enviar órdenes de pedido a Tango Gestión.

A continuación, explicamos la configuración a aplicar desde nexo Tiendas, para cargar órdenes de pedido a través de una interfaz API.

Presione el botón &quot;Obtener&quot; e introduzca un nombre de referencia para la **API**.

Al presionar el botón &quot;Aceptar&quot; se generará un Access token con el cual se podrá conectar la **API** con **nexo Tiendas**.

![imagen api](https://github.com/TangoSoftware/ApiTiendas/blob/master/api.jpg)

A partir de ese momento ya puede comenzar a utilizar la **API** en  **nexo Tiendas**  y manejar sus ventas desde  **Tango Gestión**.

La URL del servicio de API es:

[https://tiendas.axoft.com/api/v2/Aperture/order](https://tiendas.axoft.com/api/v2/Aperture/order)

Tenga en cuenta los siguientes temas:

+ [Notificaciones](#notificaciones)

+ [Preguntas frecuentes](#faqs)


<a name="notificaciones"></a>
#### Notificaciones

Si desea recibir notificaciones, en la configuración de la **API** debe marcar el check y configurar una URL donde recibirá las notificaciones.

Se enviarán notificaciones a la URL configurada de los siguientes eventos:

• Al generar el pedido de una orden de pedido web. (Se enviará el Tópico: OrderProcessed)

• Al rechazar una orden de pedido. (Se enviará el Tópico: OrderRejected)

• Al facturar el pedido generado. (Se enviará el Tópico: OrderBilled)

**Formato de json de notificación:**

{

  &quot;Topic&quot;: &quot; OrderProcessed&quot;,

  &quot;Resource&quot;: &quot;1&quot;

}


<a name="faqs"></a>
#### Preguntas Frecuentes



- **¿Cómo debo armar el JSON para cargar una orden a través de la API?**

| En la solapa API en nexo Tiendas se muestra un Ejemplo del JSON. Además, se puede ver un Modelo, Respuesta, Notificación y Respuesta notificación.Datos del Json. |
| --- |

- **¿El Access token se genera una sola vez?**

| Se genera un Access token por cuenta. Si se elimina la cuenta, al crear una nueva se generará un nuevo Access token.                  |
| --- |

- **¿Qué pasa si elimino el Access token?**

| Perderá el acceso para enviar órdenes de pedidos desde su API a Tango Gestión y estas tampoco serán recibidas en Revisión de pedidos web. |
| --- |

- **¿A nombre de quién se emite la factura de venta?**

| Cuando en la orden de pedido viene informado el número del C.U.I.L / C.U.I.T. ó D.N.I.  y se corresponden con datos de A.F.I.P., será considerada esta información para emitir la factura de ventas en la ausencia de esta información se tomará el Nombre Comercial indicado.  Cuando no se informa el número del C.U.I.L / C.U.I.T. ó  D.N.I.  se utilizará el nombre y apellido ingresado en la orden de pedido. |
| --- |

<a name="djson"></a>
### Datos del Json
[<sub>Volver</sub>](#inicio)

A continuación, se detalla a modo orientativo, el contenido de cada uno de los datos del json



**Tópico principal**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.



| **Campo** | **Requerido** | **Descripción** | **Tipo de Dato** | **Valores Posibles / Ejemplos** |
| --- | --- | --- | --- | --- |
| **Date** | Si | Fecha de la orden. Puede ser anterior a 7 días de la fecha actual. | Datetime | DD/MM/YYYY |
| **Total** | Si | Es el importe total de la orden. Sólo válido en pesos argentinos. | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;0∑ [(OrderItems.Quantity x OrderItems.UnitPrice) – OrderItems.DiscountPorcentage)] + Shipping.ShippingCost + Principal.FinancialSurcharge – Principal.TotalDiscount  |
| **TotalDiscount** | No | Importe de descuento total de la operación.  Sólo valido en pesos argentinos.   | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;=0&lt; Principal.Total |
| **PaidTotal** | Solo si se informa el tópico Payments o CashPayment | Importe total pagado. Sólo válido en pesos argentinos. | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;=0∑ (Payments.Installments \* Payments.InstallmentsAmount) + CashPayment.PaymentTotal |
| **FinancialSurcharge** | No | Importe del recargo financiero.  Sólo válido en pesos argentinos.  | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;= 0 |
| **OrderID** | Si | Identificador de la orden. Debe ser distinto para cada operación. | Alfanumérico de hasta 200 caracteres | &gt;0 |
| **OrderNumber** | Si | Número de la orden.  Es el número con el cual podrá identificar la orden desde revisión de pedidos web | Alfanumérico de hasta 200 caracteres |   |
| **CancelOrden** | No | Indica que la orden está cancelada | De tipo lógico | True/False |



**Tópico Customer**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.



| **Campo** | **Requerido** | **Descripción** | **Tipo de Dato** | **Valores Posibles / Ejemplos** |
| --- | --- | --- | --- | --- |
| **CustomerId** | Si | Identificador del cliente. | Numérico de tipo entero hasta 50 posiciones | &gt;0 |
| **DocumentType** | Si | Código del tipo de documento. | Numérico con longitud de 2 posiciones | Ver Tablas de Referencia, [Tipo de Documento](#tipodoc). |
| **DocumentNumber** | No | Número de documento sin símbolos ni puntuaciones. | Alfanumérico de hasta 20 caracteres |   |
| **User** | Si | Usuario de la tienda. | Alfanumérico de hasta 200 caracteres |   |
| **BussinessName** | No | Razón social del cliente a nombre de quién se emitirá la factura. | Alfanumérico de hasta 200 caracteres |   |
| **FirstName y LastName** | No | Nombre y apellido del cliente. Los cuales se utilizarán para emitir la factura si mediante el C.U.I.L / C.U.I.T. / D.N.I. no se encontraron datos en la A.F.I.P. | Alfanumérico.  Ambos datos pueden ocupar hasta 200 caracteres.    |   |
| **Street**** HouseNumber ****Floor**** Apartment ****City** | No | Dirección del cliente. | Alfanumérico de hasta 200 caracteres |   |
| **Email** | Si | Correo electrónico del cliente.   | Alfanumérico de hasta 255 caracteres | cliente@mail.com |
| **Comments** | No | Comentarios realizados por el cliente. | Alfanumérico de hasta 280 caracteres |   |
| **MobilePhoneNumber** | No | Número de celular del cliente. | Alfanumérico de hasta 30 caracteres |   |
| **BusinessAdress** | No | Dirección comercial del cliente. | Alfanumérico de hasta 255 caracteres |   |
| **ProvinceCode** | Si | Código A.F.I.P. con la cual se identifica la provincia del cliente. | Alfanumérico de hasta 4 caracteres | Ver Tablas de Referencia, [Provincias](#provincias). |
| **PostalCode** | No | Código postal del domicilio del cliente | Alfanumérico de hasta 10 caracteres |   |
| **PhoneNumber1** | No | Número de teléfono del cliente. | Alfanumérico de hasta 30 caracteres |   |
| **PhoneNumber2** | No | Número de teléfono del cliente. | Alfanumérico de hasta 30 caracteres |   |
| **IvaCategoryCode** | Si | Código de Categoría de I.V.A. del cliente | Alfanumérico de hasta 3 caracteres | Ver Tablas de Referencia, [Condición Fiscal](#cfiscal). |



**Tópico OrderItems**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.



| **Campo** | **Requerido** | **Descripción** | **Tipo de Dato** | **Valores Posibles / Ejemplos** |
| --- | --- | --- | --- | --- |
| **ProductCode** | Si | Código del artículo de la publicación. | Alfanumérico de hasta 200 caracteres | &lt;&gt;Vacío |
| **SKUCode** | No | Código del artículo de Tango Gestión (se refiere al que se guarda en el campo  STA11.Cod\_Sta11 de las tablas de Tango Gestión)  | Alfanumérico de hasta 17 caracteres |   |
| **VariantCode** | No | Código del artículo que representa una combinación. | Alfanumérico de hasta 200 caracteres |   |
| **Description** | No | Descripción del artículo. | Alfanumérico de hasta 400 caracteres |   |
| **VariantDescription** | No | Descripción del artículo que representa una variación. | Alfanumérico de hasta 400 caracteres |   |
| **Quantity** | Si | Cantidad del artículo. | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales |        &gt;0 |
| **UnitPrice** | Si | Precio unitario.   | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;0  |
| **DiscountPercentage** | No | Porcentaje de descuento aplicado al artículo. | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;=0  |





**Tópico Shipping**

Este tópico se completa siempre que se requiere informar el envío.  Se puede completar ya sea que el envío sea con o sin costo para el comprador.



| **Campo** | **Requerido** | **Descripción** | **Tipo de Dato** | **Valores Posibles / Ejemplos** |
| --- | --- | --- | --- | --- |
| **ShippingID** | Si | Identificador del envío. Debe ser distinto para cada operación. | Numérico de tipo entero hasta 50 posiciones. | &gt;0 |
| **ShippingCost** | No | Importe correspondiente al costo de envío. | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales. | &gt;=0 |
| **Street**** HouseNumber ****Floor**** Apartment ****City**   | No | Dirección del cliente. | Alfanumérico de hasta 200 caracteres |   |
| **PostalCode** | No | Código postal de la dirección del cliente. | Alfanumérico de hasta 10 caracteres. |   |
| **ProvinceCode** | Si | Código A.F.I.P. con la cual se identifica la provincia del cliente. | Alfanumérico de hasta 4 caracteres. | Ver Tablas de Referencia, [Provincias](#provincias). |
| **PhoneNumber1** | No | Número de teléfono del cliente. | Alfanumérico de hasta 100 caracteres |   |
| **PhoneNumber2** | No | Número de teléfono del cliente. | Alfanumérico de hasta 100 caracteres |   |
| **DeliversMonday** | No | Entrega lunes | Alfanumérico de hasta 1 caracteres | &quot;S&quot;/&quot;N&quot;Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversTuesday**   | No | Entrega martes | Alfanumérico de hasta 1 caracteres | &quot;S&quot;/&quot;N&quot;Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversWednesday** | No | Entrega miércoles | Alfanumérico de hasta 1 caracteres | &quot;S&quot;/&quot;N&quot;Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversThursday** | No | Entrega jueves | Alfanumérico de hasta 1 caracteres | &quot;S&quot;/&quot;N&quot;Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversFriday** | No | Entrega viernes | Alfanumérico de hasta 1 caracteres | &quot;S&quot;/&quot;N&quot;Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversSaturday** | No | Entrega sábado | Alfanumérico de hasta 1 caracteres | &quot;S&quot;/&quot;N&quot;Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliversSunday** | No | Entrega domingo | Alfanumérico de hasta 1 caracteres | &quot;S&quot;/&quot;N&quot;Si se deja vacío toma como defecto el valor &quot;N&quot; |
| **DeliveryHours**   | No | Hora de entrega | Alfanumérico de hasta 100 caracteres |   |



**Tópico CashPayment**

_Recuerde_: es obligatorio cargar un registro en Payments, CashPayment o ambos.



| **Campo** | **Requerido** | **Descripción** | **Tipo de Dato** | **Valores Posibles / Ejemplos** |
| --- | --- | --- | --- | --- |
| **PaymentID** | Si | Identificador del pago. Debe ser distinto para cada operación. Incluso con PaymentsID si se combina con tarjetas. | Numérico de tipo entero hasta 50 posiciones. | &gt;0 |
| **PaymentMethod** | Si | Código de Forma de Pago. | Alfanumérico de hasta 3 caracteres. | Ver Tablas de Referencia, [Formas de Pago](#fpago). |
| **PaymentTotal** | Si | Total, del pago.   | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales. | &gt;0  |





**Tópico Payments**

_Recuerde_: es obligatorio cargar un registro en Payments, CashPayment o ambos.



| **Campo** | **Requerido** | **Descripción** | **Tipo de Dato** | **Valores Posibles / Ejemplos** |
| --- | --- | --- | --- | --- |
| **PaymentsId** | Si | Identificador del pago. Debe ser distinto para cada operación. Incluso con PaymentID si se combina con efectivo. | Numérico de tipo entero hasta 50 posiciones. | &gt;0 |
| **TransactionDate** | Si | Fecha en que se realizó el pago. | Datetime | &gt;Principal.DateDD/MM/YYYY |
| **AuthorizationCode** | No | Código de autorización del pago de tarjeta. | Alfanumérico de hasta 8 caracteres |   |
| **TransactionNumber** | No | Número de transacción de pago. | Alfanumérico de hasta 40 caracteres |   |
| **Installments** | Si | Cantidad de cuotas. | Numérico hasta 2 posiciones | &gt;0 |
| **InstallmentsAmount** | Si | Importe correspondiente a la cuota. | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;0 |
| **Total** | Si | Total, del pago.   | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC].  Usando el punto como separador de decimales | &gt;0Installments \* InstallmentsAmount |
| **CardCode** | Si | Código de la tarjeta de crédito. | Alfanumérico de hasta 3 caracteres | Código de la tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Tarjetas. |
| **CardPlanCode** | Si | Plan de la tarjeta de crédito. | Alfanumérico de hasta 10 caracteres | Código del plan de tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Planes. |
| **VoucherNro** | Si | Número de cupón de tarjeta de crédito. | Numérico hasta 8 posiciones | &gt;0 |
| **CardPromotionCode** | No | Código de promoción de la tarjeta de crédito. | Alfanumérico de hasta 10 caracteres | Código de promoción de tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Promociones. |

<a name="tablas"></a>
### Tablas de Referencia
[<sub>Volver</sub>](#inicio)

<a name="tipodoc"></a>
#### Tipo de Documento

| **Código** | **Descripción** |
| --- | --- |
| 80 | C.U.I.T. |
| 86 | C.U.I.L. |
| 87 | C.D.I. |
| 89 | L.E. |
| 90 | L.C. |
| 96 | D.N.I. |


<a name="provincias"></a>
#### Provincias

| **Código** | **Descripción** |
| --- | --- |
| 0 | CIUDAD AUTONOMA BUENOS AIRES |
| 1 | BUENOS AIRES |
| 2 | CATAMARCA |
| 3 | CORDOBA |
| 4 | CORRIENTES |
| 5 | ENTRE RIOS |
| 6 | JUJUY |
| 7 | MENDOZA |
| 8 | LA RIOJA |
| 9 | SALTA |
| 10 | SAN JUAN |
| 11 | SAN LUIS |
| 12 | SANTA FE |
| 13 | SANTIAGO DEL ESTERO |
| 14 | TUCUMAN |
| 16 | CHACO |
| 17 | CHUBUT |
| 18 | FORMOSA |
| 19 | MISIONES |
| 20 | NEUQUEN |
| 21 | LA PAMPA |
| 22 | RIO NEGRO |
| 23 | SANTA CRUZ |
| 24 | TIERRA DEL FUEGO |


<a name="cfiscal"></a>
#### Condición Fiscal

| **Código** | **Descripción** |
| --- | --- |
| CF | CONSUMIDOR FINAL |
| EX | EXENTO |
| INR | NO RESPONSABLE |
| RI | RESPONSABLE INSCRIPTO |
| RS | RESPONSABLE MONOTRIBUTISTA |


<a name="fpago"></a>
#### Formas de Pago

| **Código** | **Descripción** |
| --- | --- |
| A01 | Forma de cobro Web API 01 |
| A02 | Forma de cobro Web API 02 |
| A03 | Forma de cobro Web API 03 |
| A04 | Forma de cobro Web API 04 |
| A05 | Forma de cobro Web API 05 |
| A06 | Forma de cobro Web API 06 |
| A07 | Forma de cobro Web API 07 |
| A08 | Forma de cobro Web API 08 |
| A09 | Forma de cobro Web API 098 |
| A10 | Forma de cobro Web API 10 |
| MPA | MercadoPago Argentina |
| PPA | PayPal Argentina |
| PUA | PayU Argentina |
| TPA | Todo Pago Argentina |
