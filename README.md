# Jarvis
RECONFIGURADOR DE DVR HIKVISION Y/O TVT.
Jarvis nace con la necesidad de reconfigurar miles de DVRs alrededor del mundo, de manera confiable y eficiente, posee 3 componentes principales,
uno de ellos es la cabeza, y dos brazos, uno que conoce a los dvr de marca tvt, y el otro brazo conoce a la marca Hikvision, y finalmente la cabeza,
que administra el proceso, y ejecuta uno u otro en función de un input.
Cada brazo utiliza el SDK propio de cada DVR, anotando los errores para su posterior reprocesamiento.
Pantallas, no tiene, se cargan los datos requeridos en la base de datos Sqlite, y se ejecuta la cabeza, escrita en Java, esta, recorrerá todos los registros,
e irá reconfigurando a su paso todos los dvrs, tiene un tiempo apróx de 3 segundos por cada uno, luego lo reinicia, y el dvr se pone online con su nuevo dns, luego se 
consulta una vista con los errores, y se reprocesa si es necesario.
Pantallas:

Vista luego de ejecutar:

![image](https://user-images.githubusercontent.com/11530132/133651435-6674295d-d93f-4d42-94da-16004257c37b.png)


Datos necesesarios, para el funcionamiento, datos iniciales, + datos cargados, + datos a cargar.

![image](https://user-images.githubusercontent.com/11530132/133651212-58d4f1f7-42f0-48e4-abf8-2a8eee631d9b.png)


Códigos de errores posibles:

![image](https://user-images.githubusercontent.com/11530132/133648504-69657712-b64c-46ff-a209-08d791623ddb.png)

Definición de host:
![image](https://user-images.githubusercontent.com/11530132/133648647-1e50f925-3506-43a5-98d0-2a64b11b199a.png)

Brazo a usar, en función del masvideotype_id:

![image](https://user-images.githubusercontent.com/11530132/133648776-27f71a41-fae9-4cf6-aa66-c8dfab33be24.png)

Definición de indice interno del dvr de cada dns provider:

![image](https://user-images.githubusercontent.com/11530132/133648898-a20e9233-c66c-4ef4-bb44-10ad99866665.png)



Ejecutando Jarvis para aplicar datos de DynDns

![image](https://user-images.githubusercontent.com/11530132/133652571-fe0ee33f-e448-4da6-8c69-d293a75d48eb.png)


Al ejecutar cada brazo de manera independiente, se muestra el ayuda de que parámetros recibe.

![image](https://user-images.githubusercontent.com/11530132/133650091-d8edac58-ffaa-4b84-b692-0d9042b73e25.png)

![image](https://user-images.githubusercontent.com/11530132/133650200-63b7455f-e286-401a-9ccc-3a278f2000f6.png)






