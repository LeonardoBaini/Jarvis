# Jarvis
RECONFIGURADOR DE DVR HIKVISION Y/O TVT.
Jarvis nace con la necesidad de reconfigurar miles de DVRs alrededor del mundo, de manera confiable y eficiente, posee 3 componentes principales,
uno de ellos es la cabeza, y dos brazos, uno que conoce a los dvr de marca tvt, y el otro brazo conoce a la marca Hikvision, y finalmente la cabeza,
que administra el proceso, y ejecuta uno u otro en función de un input.
Cada brazo utiliza el SDK propio de cada DVR, anotando los errores para su posterior reprocesamiento.
Pantallas, no tiene, se cargan los datos requeridos en la base de datos Sqlite, y se ejecuta la cabeza, escrita en Java, esta, recorrerá todos los registros,
e irá reconfigurando a su paso todos los dvrs, tiene un tiempo apróx de 3 segundos por cada uno, luego lo reinicia, y el dvr se pone online con su nuevo dns, luego se 
consulta una vista con los errore, y se reprocesa si es necesario.
Pantallas:

Vista luego de ejecutar:
