Patrones de diseño utilizados:

Patrón 1: Singleton

Utilice este patrón para usar el input manager desde varios lugares sin tener que serializar una gran cantidad de veces, además algo tan global como el input me interesa que se controle desde un lugar solo y de esta forma me aseguro que solo se cree una vez. 

Patrón 2: State

Este patrón lo aplique en el game manager para maniobrar todo el flujo de juego desde una forma más organizada teniendo en cuenta la calibración, inicialización, modo carrera y game over. A su vez en el game manager también se aplica un singleton para ser accesible de forma estática y así cambiar de estado más fácilmente.

Patron 3: Object Pool

Este esquema generalizado lo utilice en las bolsas de dinero, filas de cajas de madera y depósitos, con el propósito de instanciar una cierta cantidad según la dificultad seleccionada en un script aparte llamado SpawManager.
