<a name="inicio"></a>
Tango Software - API Tango Tiendas
=======

- [Instalación](#instalacion)
  - [Versiones soportadas de Tango Tiendas](#versiones)
  - [Generalidades](#generalidades)
  - [Ambientes](#ambientes)
  - [Asociar aplicación con API](#asociarapi)
- [Recepción de órdenes API](#ordenes)
  - [Datos del JSON](#djson)
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

##### Recepción de órdenes API

La versión requerida de ventas para implementar Tango Tiendas API es: 18.01.000.3567 (o superior).  
correspondiente a al hotfix ftp://ftp.axoft.com/version_interna/HotfixPublicados/HotFix_18_01_000_0983.exe (o posterior)
Es necesario contar con el producto Tango Gestión y Tango Punto de Venta Argentina y el módulo de tesorería activado.  
Es necesario contar con la aplicación **Tango Tiendas** activada.

##### Consulta de datos

Los datos comienzan a estar disponibles cuando se cumplen las siguientes condiciones:  
La versión requerida de ventas para implementar Tango Tiendas API es: 19.01.000.605 (o superior).  
correspondiente al hotfix ftp://ftp.axoft.com/version_interna/HotfixPublicados/HotFix_19_01_000_0437.exe (o posterior)
Es necesario que la licencia de **Tango Gestión y Tango Punto de Venta Argentina** tenga la aplicación **Tango Tiendas Full** activada.

<a name="generalidades"></a>

#### Generalidades

[<sub>Volver</sub>](#inicio)

Para utilizar **Tango Tiendas** debe tener instalada la versión vigente del sistema o la inmediata anterior. Comuníquese con su distribuidor para mayor información.

Esta versión soporta órdenes de pedido únicamente en moneda nacional argentina.

Aceptando hasta 2 decimales en los datos de importes y precios.

<a name="ambientes"></a>

#### Ambientes

[<sub>Volver</sub>](#inicio)

• Ambiente de testeo

Para configurar el ambiente de testeo desde Tango Sync debe asociar una empresa de nube con una empresa ejemplo de Tango Gestión o Tango Punto de Venta.

• Ambiente de producción

Para configurar el ambiente de producción desde Tango Sync debe asociar una empresa de nube con una empresa operativa de Tango Gestión o Tango Punto de Venta.

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

[https://tiendas.axoft.com/api/v2/Aperture/dummy](https://tiendas.axoft.com/api/v2/Aperture/dummy)

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

[https://tiendas.axoft.com/api/v2/Aperture/order](https://tiendas.axoft.com/api/v2/Aperture/order)

Tenga en cuenta los siguientes temas:

- [Notificaciones](#notificaciones)

- [Preguntas frecuentes](#faqs)

<a name="notificaciones"></a>

#### Notificaciones

Si desea recibir notificaciones, en la configuración de la **API** debe marcar el check y configurar una URL donde recibirá las notificaciones.

Se enviarán notificaciones a la URL configurada de los siguientes eventos:

• Al generar el pedido de una orden de pedido Tango Tiendas. (Se enviará el Tópico: OrderProcessed)

• Al rechazar una orden de pedido. (Se enviará el Tópico: OrderRejected)

• Al facturar el pedido generado. (Se enviará el Tópico: OrderBilled)

**Formato de JSON de notificación:**

```
{

  "Topic": "OrderProcessed",

  "Resource": "1"

}
```

<a name="faqs"></a>

#### Preguntas Frecuentes

- **¿Cómo debo armar el JSON para cargar una orden a través de la API?**

En la solapa API en Tango Tiendas se muestra un Ejemplo del JSON. Además, se puede ver un Modelo, Respuesta, Notificación y Respuesta notificación.Datos del JSON.

- **¿El Access token se genera una sola vez?**

Se genera un Access token por cuenta. Si se elimina la cuenta, al crear una nueva se generará un nuevo Access token.

- **¿Qué pasa si elimino el Access token?**

Perderá el acceso para enviar órdenes de pedidos desde su API a Tango y estas tampoco serán recibidas en Revisión de pedidos de Tango Tiendas.

- **¿A nombre de quién se emite la factura de venta?**

Cuando en la orden de pedido viene informado el número del C.U.I.L / C.U.I.T. ó D.N.I. y se corresponden con datos de A.F.I.P., será considerada esta información para emitir la factura de ventas en la ausencia de esta información se tomará el Nombre Comercial indicado. Cuando no se informa el número del C.U.I.L / C.U.I.T. ó D.N.I. se utilizará el nombre y apellido ingresado en la orden de pedido.

<a name="djson"></a>

#### Novedades en Json de la orden

Ahora en los datos del json se puede especificar los siguientes campos:  

  • SaleConditionCode: Condición de Venta

  • TranportCode: Código del transporte

  • SellerCode: Código del vendedor


#### Consideraciones al enviar órdenes

- **Condición de venta**

Si la condición de venta es distinto de 'Contado', es posible que al valor de la factura se le apliquen cargos propios de dicha condición (Ej. 30/60/90 días con un 2% de interes).

- **Transporte**

Si la "Condicíón de Venta" es 'Contado' (o en su defecto no se informa), entonces se válida que el código de tranporte informado no tenga recargo (SurchargePercentage = 0).

- **Pagos**

Si la "Condición de Venta" es distinto de 'Contado', entonces se válida que no se informen los tópicos de:    
  • CashPayment
  • Payments

- **General**

Si ninguno de estos códigos se informan, se mantiene el comportamiento actual.


### Datos del JSON

[<sub>Volver</sub>](#inicio)

A continuación, se detalla a modo orientativo, el contenido de cada uno de los datos del JSON

**Tópico principal**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.

| **Campo**                      | **Requerido**                                       | **Descripción**                                                                                                     | **Tipo de Dato**                                                                                       | **Valores Posibles / Ejemplos**                                                                                                                                         |
| ------------------------------ | --------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **OrderID**                    | Si                                                  | Identificador de la orden. Debe ser distinto para cada operación.                                                   | Alfanumérico de hasta 200 caracteres                                                                   | &gt;0                                                                                                                                                                   |
| **OrderNumber**                | Si                                                  | Número de la orden. Es el número con el cual podrá identificar la orden desde revisión de pedidos de Tango Tiendas  | Alfanumérico de hasta 200 caracteres                                                                   |                                                                                                                                                                         |
| **Date**                       | Si                                                  | Fecha de la orden. Puede ser anterior a 7 días de la fecha actual.                                                  | Datetime                                                                                               | DD/MM/YYYY hh:mm:ss                                                                                                                                                     |
| **Total**                      | Si                                                  | Es el importe total de la orden. Sólo válido en pesos argentinos.                                                   | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0 ∑[(OrderItems.Quantity x OrderItems.UnitPrice) – OrderItems.DiscountPorcentage)] + Shipping.ShippingCost + Principal.FinancialSurcharge – Principal.TotalDiscount |
| **TotalDiscount**              | No                                                  | Importe de descuento total de la operación. Sólo valido en pesos argentinos.                                        | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;=0&lt; Principal.Total                                                                                                                                              |
| **PaidTotal**                  | Solo si se informa el tópico Payments o CashPayment | Importe total pagado. Sólo válido en pesos argentinos.                                                              | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;=0 ∑(Payments.Installments \* Payments.InstallmentsAmount) + CashPayment.PaymentTotal                                                                               |
| **FinancialSurcharge**         | No                                                  | Importe del recargo financiero. Sólo válido en pesos argentinos.                                                    | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;= 0                                                                                                                                                                 |
| **WarehouseCode**              | No                                                  | Código del depósito. Si el depósito no existe o está inhabilitado en Tango, no se podrá generar el pedido.          | Alfanumérico de hasta 2 caracteres                                                                     |
| **SellerCode**                 | No                                                  | Código del vendedor. Si el vendedor no existe o está inhabilitado en Tango, no se podrá generar el pedido.          | Alfanumérico de hasta 12 caracteres                                                                    |                                                                                                                                                                         |
| **TransportCode**              | No                                                  | Código del transporte. Si el transporte no existe o está inhabilitado en Tango, no se podrá generar el pedido.      | Alfanumérico de hasta 12 caracteres                                                                    |                                                                                                                                                                         |
| **SaleConditionCode**        | No                                                  | Condición de venta. Si la condición de venta no existe o está inhabilitado en Tango, no se podrá generar el pedido. | Numérico de tipo entero hasta 10 posiciones                                                            |                                                                                                                                                                         |
| **CancelOrden**                | No                                                  | Indica que la orden está cancelada                                                                                  | De tipo lógico                                                                                         | True/False                                                                                                                                                              |
| **ValidateTotalWithPaidTotal** | Si                                                  | Indica si al momento de enviar la orden se valida el total de la orden con el total pagado.                         | De tipo lógico                                                                                         | True/False                                                                                                                                                              |

**Tópico Customer**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.

| **Campo**             | **Requerido** | **Descripción**                                                                                                                             | **Tipo de Dato**                            | **Valores Posibles / Ejemplos**                          |
| --------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------- | -------------------------------------------------------- |
| **CustomerId**        | Si            | Identificador del cliente.                                                                                                                  | Numérico de tipo entero hasta 10 posiciones | &gt;0                                                    |
| **DocumentType**      | Si            | Código del tipo de documento.                                                                                                               | Numérico con longitud de 2 posiciones       | Ver Tablas de Referencia, [Tipo de Documento](#tipodoc). |
| **DocumentNumber**    | No            | Número de documento sin símbolos ni puntuaciones.                                                                                           | Alfanumérico de hasta 20 caracteres         |                                                          |
| **User**              | Si            | Usuario de la tienda.                                                                                                                       | Alfanumérico de hasta 200 caracteres        |                                                          |
| **BussinessName**     | No            | Razón social del cliente a nombre de quién se emitirá la factura.                                                                           | Alfanumérico de hasta 200 caracteres        |                                                          |
| **FirstName**         | No            | Nombre del cliente. Se utilizará para emitir la factura si mediante el C.U.I.L / C.U.I.T. / D.N.I. no se encontraron datos en la A.F.I.P.   | Alfanumérico de hasta 200 caracteres        |                                                          |
| **LastName**          | No            | Apellido del cliente. Se utilizará para emitir la factura si mediante el C.U.I.L / C.U.I.T. / D.N.I. no se encontraron datos en la A.F.I.P. | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Street**            | No            | Calle del domicilio del cliente.                                                                                                            | Alfanumérico de hasta 200 caracteres        |                                                          |
| **HouseNumber**       | No            | Altura del domicilio del cliente.                                                                                                           | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Floor**             | No            | Piso del domicilio del cliente.                                                                                                             | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Apartment**         | No            | Departamento del domicilio del cliente.                                                                                                     | Alfanumérico de hasta 200 caracteres        |                                                          |
| **City**              | No            | Localidad del domicilio del cliente.                                                                                                        | Alfanumérico de hasta 200 caracteres        |                                                          |
| **Email**             | Si            | Correo electrónico del cliente.                                                                                                             | Alfanumérico de hasta 255 caracteres        | cliente@mail.com                                         |
| **Comments**          | No            | Comentarios realizados por el cliente.                                                                                                      | Alfanumérico de hasta 280 caracteres        |                                                          |
| **MobilePhoneNumber** | No            | Número de celular del cliente.                                                                                                              | Alfanumérico de hasta 30 caracteres         |                                                          |
| **BusinessAdress**    | No            | Dirección comercial del cliente.                                                                                                            | Alfanumérico de hasta 255 caracteres        |                                                          |
| **ProvinceCode**      | Si            | Código A.F.I.P. con la cual se identifica la provincia del cliente.                                                                         | Alfanumérico de hasta 4 caracteres          | Ver Tablas de Referencia, [Provincias](#provincias).     |
| **PostalCode**        | No            | Código postal del domicilio del cliente                                                                                                     | Alfanumérico de hasta 8 caracteres          |                                                          |
| **PhoneNumber1**      | No            | Número de teléfono del cliente.                                                                                                             | Alfanumérico de hasta 30 caracteres         |                                                          |
| **PhoneNumber2**      | No            | Número de teléfono del cliente.                                                                                                             | Alfanumérico de hasta 30 caracteres         |                                                          |
| **IvaCategoryCode**   | Si            | Código de Categoría de I.V.A. del cliente                                                                                                   | Alfanumérico de hasta 3 caracteres          | Ver Tablas de Referencia, [Condición Fiscal](#cfiscal).  |

**Tópico OrderItems**

_Recuerde_: es obligatorio cargar un registro en este tópico para generar una orden.

| **Campo**              | **Requerido** | **Descripción**                                                                                                               | **Tipo de Dato**                                                                                       | **Valores Posibles / Ejemplos**                                                                           |
| ---------------------- | ------------- | ----------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------- |
| **ProductCode**        | Si            | Código del artículo de la publicación.                                                                                        | Alfanumérico de hasta 200 caracteres                                                                   | &lt;&gt;Vacío. Debe ser único si la publicación no se trata de un artículo con escala.[Ejemplo](#Ejemplo) |
| **SKUCode**            | No            | Código del artículo de Tango Gestión (se refiere al que se guarda en el campo STA11.Cod_Sta11 de las tablas de Tango Gestión) | Alfanumérico de hasta 17 caracteres                                                                    |                                                                                                           |
| **VariantCode**        | No            | Código del artículo que representa una combinación.                                                                           | Alfanumérico de hasta 200 caracteres                                                                   |                                                                                                           |
| **Description**        | Sí            | Descripción del artículo.                                                                                                     | Alfanumérico de hasta 400 caracteres                                                                   |                                                                                                           |
| **VariantDescription** | No            | Descripción del artículo que representa una variación.                                                                        | Alfanumérico de hasta 400 caracteres                                                                   |                                                                                                           |
| **Quantity**           | Si            | Cantidad del artículo.                                                                                                        | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0                                                                                                     |
| **UnitPrice**          | Si            | Precio unitario.                                                                                                              | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0                                                                                                     |
| **DiscountPercentage** | No            | Porcentaje de descuento aplicado al artículo.                                                                                 | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;=0                                                                                                    |

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

**Tópico Shipping**

Este tópico se completa siempre que se requiere informar el envío. Se puede completar ya sea que el envío sea con o sin costo para el comprador.

| **Campo**             | **Requerido** | **Descripción**                                                     | **Tipo de Dato**                                                                                        | **Valores Posibles / Ejemplos**                                 |
| --------------------- | ------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------- |
| **ShippingID**        | Si            | Identificador del envío. Debe ser distinto para cada operación.     | Numérico de tipo entero hasta 50 posiciones.                                                            | &gt;0                                                           |
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

**Tópico CashPayment**

_Recuerde_: si no carga un registro en Payments, CashPayment o ambos, deberá completar la forma de cobro al momento de emitir la factura. Por otro lado, si lo que se envia es una modificación de una órden la cual antes contenía el tópico CashPayment y ahora no, se procederá a cancelar el pago anterior.

| **Campo**         | **Requerido** | **Descripción**                                                                                                   | **Tipo de Dato**                                                                                        | **Valores Posibles / Ejemplos**                     |
| ----------------- | ------------- | ----------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- | --------------------------------------------------- |
| **PaymentID**     | Si            | Identificador del pago. Debe ser distinto para cada operación. Incluso con PaymentsID si se combina con tarjetas. | Numérico de tipo entero hasta 50 posiciones.                                                            | &gt;0                                               |
| **PaymentMethod** | Si            | Código de Forma de Pago.                                                                                          | Alfanumérico de hasta 3 caracteres.                                                                     | Ver Tablas de Referencia, [Formas de Pago](#fpago). |
| **PaymentTotal**  | Si            | Total, del pago.                                                                                                  | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales. | &gt;0                                               |

**Tópico Payments**

_Recuerde_: si no carga un registro en Payments, CashPayment o ambos, deberá completar la forma de cobro al momento de emitir la factura. Por otro lado, si lo que se envia es una modificación de una órden la cual antes contenía un pago que ahora no, se procederá a cancelar el pago anterior no enviado en la modificación.

| **Campo**              | **Requerido** | **Descripción**                                                                                                  | **Tipo de Dato**                                                                                       | **Valores Posibles / Ejemplos**                                                                                                                          |
| ---------------------- | ------------- | ---------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **PaymentsId**         | Si            | Identificador del pago. Debe ser distinto para cada operación. Incluso con PaymentID si se combina con efectivo. | Numérico de tipo entero hasta 50 posiciones.                                                           | &gt;0                                                                                                                                                    |
| **TransactionDate**    | Si            | Fecha en que se realizó el pago.                                                                                 | Datetime                                                                                               | &gt;Principal.Date DD/MM/YYYY hh:mm:ss                                                                                                                   |
| **AuthorizationCode**  | No            | Código de autorización del pago de tarjeta.                                                                      | Alfanumérico de hasta 8 caracteres                                                                     |                                                                                                                                                          |
| **TransactionNumber**  | No            | Número de transacción de pago.                                                                                   | Alfanumérico de hasta 40 caracteres                                                                    |                                                                                                                                                          |
| **Installments**       | Si            | Cantidad de cuotas.                                                                                              | Numérico hasta 2 posiciones                                                                            | &gt;0                                                                                                                                                    |
| **InstallmentsAmount** | Si            | Importe correspondiente a la cuota.                                                                              | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0                                                                                                                                                    |
| **Total**              | Si            | Total, del pago.                                                                                                 | Numérico con 13 dígitos con hasta 2 decimales 999999[.CC]. Usando el punto como separador de decimales | &gt;0Installments \* InstallmentsAmount                                                                                                                  |
| **CardCode**           | Si            | Código de la tarjeta de crédito.                                                                                 | Alfanumérico de hasta 3 caracteres                                                                     | Código de la tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Tarjetas.              |
| **CardPlanCode**       | Si            | Plan de la tarjeta de crédito.                                                                                   | Alfanumérico de hasta 10 caracteres                                                                    | Código del plan de tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Planes.          |
| **VoucherNro**         | Si            | Número de cupón de tarjeta de crédito.                                                                           | Numérico hasta 8 posiciones                                                                            | &gt;0                                                                                                                                                    |
| **CardPromotionCode**  | No            | Código de promoción de la tarjeta de crédito.                                                                    | Alfanumérico de hasta 10 caracteres                                                                    | Código de promoción de tarjeta de crédito de Tango Gestión Se localiza en la opción de menú del módulo de Tesorería / Archivos / Tarjetas / Promociones. |

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

| **Código** | **Descripción**            |
| ---------- | -------------------------- |
| CF         | CONSUMIDOR FINAL           |
| EX         | EXENTO                     |
| INR        | NO RESPONSABLE             |
| RI         | RESPONSABLE INSCRIPTO      |
| RS         | RESPONSABLE MONOTRIBUTISTA |

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
  "OrderID": "75906",
  "OrderNumber": "75906",
  "ValidateTotalWithPaidTotal": true,
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
  "CashPayment": {
    "PaymentID": 38566912,
    "PaymentMethod": "A02",
    "PaymentTotal": 123.0
  },
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

### Ejemplo de JSON de una órden (Condición de venta - cuenta corriente)

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

<a name="subida"></a>

## Consulta de datos

[<sub>Volver</sub>](#inicio)

La consulta de datos se basa en una serie de servicios que permiten consultar datos obtenidos de **Tango Gestión o Tango Punto de Venta** y devolviendo como resultado, la respuesta en formato JSON.

<a name="configuracion"></a>

### Configuración

[<sub>Volver</sub>](#inicio)

Previo a comenzar a utilizar los servicios de consulta debe verificar la configuración general de **Tango Tiendas**. Allí debe indicar mediante la configuración "Centraliza stock de varias sucursales", si los saldos de stock se tomarán desde central o desde la sucursal de la empresa asociada.

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
- No posean doble unidad de medida.

| **Recurso**                                                                     |
| ------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Product?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                                  | **GET**                                                                            |
| --------------------------------------------------------- | ---------------------------------------------------------------------------------- |
| Obtener los artículos cuyo código contenga la cadena "01" | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1&filter=01 |
| Obtener todos los artículos                               | https://tiendas.axoft.com/api/Aperture/Product?pageSize=500&pageNumber=1           |

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
            "SKUCode": "010030001BLA",
            "Description": "VENT.DE TECHO MADERA 3 PALAS",
            "AdditionalDescription": "MADERA PINT.BLAN.",
            "AlternativeCode": "",
            "BarCode": "",
            "Commission": 0,
            "Discount": 0,
            "MeasureUnitCode": "C/U",
            "MaximumStock": 32,
            "MinimumStock": 4,
            "RestockPoint": 6,
            "Observations": "",
            "Kit": false,
            "KitValidityDateSince": null,
            "KitValidityDateUntil": null,
            "UseScale": "S",
            "Scale1": "TI",
            "Scale2": "CO",
            "BaseArticle": "010030",
            "ScaleValue1": "001",
            "ScaleValue2": "BLA",
            "DescriptionScale1": "TIPO DE VENTILADORES",
            "DescriptionScale2": "COLORES",
            "DescriptionValueScale1": "MADERA",
            "DescriptionValueScale2": "BLANCO",
            "Disabled": false,
            "ProductComposition": [],
            "ProductComments": []
        }
    ]
}
```

<a name="clientes"></a>

#### Clientes

[<sub>Volver</sub>](#iniciorecursos)

Permite obtener datos de clientes, con sus direcciones de entrega y comentarios.

| **Recurso**                                                                      |
| -------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Customer?{pageSize}&{pageNumber}&[filter] |

Ejemplos

| **Para**                                                 | **GET**                                                                             |
| -------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| Obtener los clientes cuyo código contenga la cadena "CL" | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1&filter=CL |
| Obtener todos los clientes                               | https://tiendas.axoft.com/api/Aperture/Customer?pageSize=500&pageNumber=1           |

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
            "DocumentType": "4",
            "DocumentNumber": "",
            "PriceListNumber": 1,
            "Discount": 15,
            "Observations": "",
            "DisabledDate": null,
            "SellerCode": "1",
            "CreditQuota": 15000.00,
            "LocalAccountBalance": 6500.50,
            "ForeignAccountBalance": 30.00,
            "ForeignCurrencyClause": false,
            "CreditQuotaCurrencyCode": "1",
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
            "CustomerComments": []
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
            ]
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
            "CustomerComments": []
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

| **Para**                                              | **GET**                                                                             |
| ----------------------------------------------------- | ----------------------------------------------------------------------------------- |
| Obtener la lista de precios cuyo número de lista es 1 | https://tiendas.axoft.com/api/Aperture/PriceList?pageSize=500&pageNumber=1&filter=1 |
| Obtener todas las listas de precios                   | https://tiendas.axoft.com/api/Aperture/PriceList?pageSize=500&pageNumber=1          |

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
  - No posean doble unidad de medida.
  - No estén inhabilitados.
- Listas de precios que en **Tango Gestión y Tango Punto de Venta Argentina** no estén inhabilitadas.

| **Recurso**                                                                             |
| --------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Price?{pageSize}&{pageNumber}&[filter]&[SKUCode] |

Ejemplos

| **Para**                                                                                                             | **GET**                                                                                    |
| -------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------ |
| Obtener los precios de la lista de precios cuyo número de lista es 1                                                 | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1&filter=1            |
| Obtener los precios de la lista de precios cuyo número de lista es 1 y el código de artículo contenga la cadena "01" | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1&filter=1&SKUCode=01 |
| Obtener todos los precios                                                                                            | https://tiendas.axoft.com/api/Aperture/Price?pageSize=500&pageNumber=1                     |

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
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100100134",
            "Price": 20215.55,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100100265",
            "Price": 30330.55,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100200528",
            "Price": 87408.05,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100200530",
            "Price": 53465,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0100200630",
            "Price": 41168.05,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200100013",
            "Price": 27.51,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200100124",
            "Price": 98.26,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200200033",
            "Price": 41.62,
            "ValidityDateSince": "2018-01-01T00:00:00",
            "ValidityDateUntil": "2019-01-01T00:00:00"
        },
        {
            "PriceListNumber": 1,
            "SKUCode": "0200200034",
            "Price": 199.46,
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
  - No posean doble unidad de medida.
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
  - No posean doble unidad de medida.
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
En el campo **Quantity** se muestra la cantidad física en stock.
En el campo **EngagedQuantity** se muestra la cantidad comprometida del stock.
En el campo **PendingQuantity** se muestra la cantidad pendiente de ingresar al stock.

Solo se mostrarán saldos de stock de:

- Artículos que en **Tango Gestión y Tango Punto de Venta Argentina** cumplan:
  - Perfil de Venta, Compra-Venta o Inhabilitado.
  - Tipo Simple, Fórmula, o Kit fijo.
  - No sean artículos Base.
  - No posean doble unidad de medida.
  - No estén inhabilitados.
- Depósitos que en **Tango Gestión y Tango Punto de Venta Argentina** no estén inhabilitados.

| **Recurso**                                                                                                                                          |
| ---------------------------------------------------------------------------------------------------------------------------------------------------- |
| https://tiendas.axoft.com/api/Aperture/Stock?{pageSize}&{pageNumber}&[filter]&[groupByProduct]&[discountPendingOrders]&[StoreNumber]&[WarehouseCode] |

Ejemplos

| **Para**                                                                                                                                                                                                                                        | **GET**                                                                                                        |
| ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------- |
| Obtener los saldos de stock de los artículos cuyo código contenga la cadena "01"                                                                                                                                                                | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&filter=01                               |
| Obtener los saldos de stock de los artículos cuyo código contenga la cadena "01", la sucursal sea 1 y el depósito corresponda al código 2. En este caso no es válido agregar la agrupación por producto (groupByProduct=true)                   | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&filter=01&StoreNumber=1&WarehouseCode=2 |
| Obtener los saldos de stock acumulados por artículo (En este caso la consulta no devolverá datos en los campos "StoreNumber" y "WarehouseCode")                                                                                                 | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&groupByProduct=true                     |
| Obtener los saldos de stock, restando al mismo las órdenes pendientes de revisión (en el caso de no solicitar agrupado por artículo, los registros de la cantidad en órdenes no devolverán datos en los campos "StoreNumber" y "WarehouseCode") | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1&discountPendingOrders=true              |
| Obtener todos los saldos de stock                                                                                                                                                                                                               | https://tiendas.axoft.com/api/Aperture/Stock?pageSize=500&pageNumber=1                                         |

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

| **Para**                      | **GET**                                                                    |
| ----------------------------- | -------------------------------------------------------------------------- |
| Obtener todos los transportes | https://tiendas.axoft.com/api/Aperture/Transport?pageSize=500&pageNumber=1 |

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
            "IVACategoryCode": "Responsable Inscripto",
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
            "IVACategoryCode": "Responsable Inscripto",
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
            "IVACategoryCode": "",
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

| **Para**                               | **GET**                                                                        |
| -------------------------------------- | ------------------------------------------------------------------------------ |
| Obtener todas las condiciones de venta | https://tiendas.axoft.com/api/Aperture/SaleCondition?pageSize=500&pageNumber=1 |

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
