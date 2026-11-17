# Validación · tres niveles de cultura ecuatoriana

Entorno: Windows, Unity 6.0 LTS `6000.0.80f1`.

## Pruebas automáticas reales

`ClassroomProject.SmokeTest` ejecutó **40 comprobaciones** en Play Mode y terminó con código 0 y `AULA_SMOKE_TEST_PASSED`. La evidencia resumida está en `RESULTADOS.txt`.

- Inicio en menú con navegación pausada y exactamente tres niveles.
- Entrada al aula, movimiento, distancia y raycast hacia la pizarra.
- Apertura y cierre de la lectura con pausa de navegación.
- Examen bloqueado hasta explorar las fichas y resolver la práctica.
- En cada nivel: práctica incorrecta con reintento, desbloqueo, imposibilidad de saltar preguntas, score perfecto 3/3, resultado 0/3, reinicio del score y conservación del mejor resultado.
- Confirmar la misma respuesta varias veces no aumenta la puntuación.
- Regresar al menú pausa el movimiento; la pizarra bloquea físicamente al jugador.

La prueba usa los mismos métodos que activa la interfaz, pero no simula clics físicos en las preguntas.

## Compilación

`ClassroomProject.PreviewAndBuild` compiló Windows x86_64 correctamente: `Succeeded`, salida 0 y `AULA_WINDOWS_BUILD_PASSED`. El ejecutable está en `Builds/AulaInclusiva3D`, excluido de Git junto con la caché de Unity. La captura `Aula.png` contiene la escena 3D, sin interfaz.

## Revisión del ejecutable independiente

Se abrió la aplicación Windows y se inspeccionó el menú con sus tres temas, ilustraciones, botones y fondo opaco. Se probó la elección del nivel 1 y el movimiento W. La revisión detectó texto recortado en los botones del menú; se corrigió la altura y se volvió a compilar. En la compilación final se verificaron los tres botones legibles y el aumento del tamaño de texto.

![Menú de tres niveles en el ejecutable final](Menu.png)

El color de fondo de lectura es sólido `RGB(0.035, 0.05, 0.075)`, alfa 1; la interfaz ya no usa el panel semitransparente predeterminado. El contenido se coloca en una vista desplazable para acomodar texto grande.

## Límites de la validación

No se completaron los tres exámenes mediante clics físicos en el ejecutable: se comprobaron automáticamente en Play Mode. Antes de exponer se recomienda recorrer clase, práctica y examen con la pantalla y dispositivos que se utilizarán. No se ha realizado una evaluación con estudiantes sordos o docentes de la comunidad sorda. El prototipo no incluye lengua de señas ecuatoriana ni declara cumplimiento de una norma de accesibilidad.

Los comandos para repetir las pruebas y la compilación están en el README.
