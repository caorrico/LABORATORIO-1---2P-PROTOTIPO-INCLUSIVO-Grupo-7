# Aula Inclusiva 3D

Prototipo universitario del Grupo 7 para personas sordas o con discapacidad auditiva. Aula en primera persona con una lección escrita sobre comunicación inclusiva. No requiere audio, micrófono, cuenta ni servicios externos.

## Abrir y ejecutar

1. Instala **Unity 6.0 LTS, versión 6000.0.80f1**, mediante Unity Hub (es la versión instalada utilizada para este proyecto).
2. Clona este repositorio. En Unity Hub selecciona **Add > Add project from disk** y elige la carpeta raíz que contiene `Assets`, `Packages` y `ProjectSettings`.
3. Espera a que termine la importación. Abre `Assets/Scenes/AulaInclusiva.unity`.
4. Pulsa **Play**, haz clic en la vista Game y camina por el pasillo central hacia la pizarra.
5. A menos de 3,2 metros, apunta al centro de la pizarra y presiona **E**. Lee las tres páginas y responde la pregunta. Puedes repetir la lectura sin límite de tiempo.

No hay que crear objetos ni asignar referencias manualmente. La escena ya está guardada y añadida a la lista de compilación. Se utiliza el renderizador integrado y el sistema de entrada clásico; no se necesita URP, TextMesh Pro ni paquetes de terceros.

## Controles

| Acción | Control |
| --- | --- |
| Caminar | W, A, S, D |
| Mirar | Ratón |
| Abrir / cerrar la lección | E |
| Cerrar lección / liberar cursor | Esc |
| Recuperar control del ratón | Clic en Game fuera de la lección |
| Cambiar página / responder | Botones con el ratón |
| Tamaño de texto de la lección | A− / A+ (22–30) |

La lectura pausa el desplazamiento y libera el cursor. Las instrucciones, avisos de proximidad y respuestas aparecen por escrito, sin depender solo del color. Las páginas no avanzan automáticamente. Resolución recomendada: 1280 × 800 o superior; la interfaz se escala en pantallas menores.

## Contenido

- Aula de 12 × 14 metros con piso, cuatro paredes, seis mesas, seis sillas, iluminación y pizarra.
- Colisiones con el aula, los muebles y la pizarra; controlador con gravedad y cámara a altura de los ojos.
- Interacción mediante raycast desde el centro de la cámara: requiere distancia y visión directa.
- Tres páginas educativas y una pregunta de comprobación con feedback escrito y reintento.
- `Assets/Scripts/FirstPersonController.cs`: cámara, WASD, gravedad y cursor.
- `Assets/Scripts/ClassroomLesson.cs`: interacción y lección visual.
- `Assets/Scripts/Blackboard.cs`: identifica la superficie interactiva.
- `Assets/Editor/ClassroomProject.cs`: generación reproducible de la escena y comprobación automática en Play Mode.
- `Assets/Models/Mesa.obj` y `Silla.obj`: modelos originales, sencillos, utilizados en la escena.

## Modelos y Blender

En Blender usa **File > Import > Wavefront (.obj)** para abrir los modelos de `Assets/Models`. Sus coordenadas se expresan en metros; no requieren texturas externas. Los colores se asignan mediante materiales de Unity. Piso, paredes y pizarra usan cubos nativos de Unity.

Los OBJ se pueden regenerar con Python 3: `python Tools/generate_models.py` (esto reemplaza ambos OBJ). Para actualizar la escena después de cambiar modelos, usa el menú **Aula Inclusiva > Regenerar escena**. **Este comando reemplaza la escena del prototipo**; guarda una copia si hiciste cambios manuales.

## Comprobación

Consulta `Documentation/VALIDACION.md` para los resultados reales y las limitaciones de las pruebas. Para repetir la comprobación con el editor cerrado, ejecuta desde PowerShell, sustituyendo las rutas:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.0.80f1\Editor\Unity.exe' -batchmode -nographics -projectPath 'C:\ruta\al\repositorio' -executeMethod ClassroomProject.SmokeTest -logFile 'C:\ruta\smoke.log'
```

No agregues `-quit` a la prueba: el método entra en Play Mode y termina con código 0 si pasa, 1 si falla. El log debe contener `AULA_SMOKE_TEST_PASSED`. La prueba comprueba movimiento, distancia, raycast, apertura y cierre de lectura, navegación, respuestas y bloqueo por colisiones. No simula teclas físicas ni evalúa legibilidad con usuarios.

Para generar un ejecutable: **File > Build Profiles**, selecciona Windows y verifica que `AulaInclusiva` está en la lista de escenas; usa **Build**. Instala el módulo Windows Build Support si Unity Hub lo solicita.

## Guion de presentación (2 minutos)

1. Explica que la actividad comunica toda su información mediante texto.
2. Recorre el pasillo y muestra el aviso de interacción al acercarte.
3. Abre la pizarra, aumenta el texto y recorre las páginas sin prisa.
4. Responde primero A para mostrar el reintento y luego B para completar la actividad.
5. Cierra la lección y vuelve a caminar.

## Alcance

Es un prototipo educativo de escritorio, no una aplicación VR ni una herramienta de enseñanza de lengua de señas. No incluye traducción automática, avatares de señas ni audio. La accesibilidad está orientada a la comunicación visual; no se afirma conformidad con normas ni validación con personas usuarias. Una futura evaluación debe realizarse con la comunidad sorda y ajustar las preferencias de comunicación a sus necesidades.

Vista de la cámara del aula (sin interfaz):

![Aula 3D](Documentation/Aula.png)
