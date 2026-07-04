import GameScene from "./GameScene.js";
import MenuScene from "./MenuScene.js";
import GameOverScene from "./GameOverScene.js";

let config = {
	type: Phaser.AUTO,
	parent: 'juego',
	pixelArt: true,
	scale: {
		autoCenter: Phaser.Scale.CENTER_HORIZONTALLY,
		mode: Phaser.Scale.FIT,
		width: 500,
		height: 300,
		zoom: 1
	},
	scene: [MenuScene, GameScene, GameOverScene],
	physics: {
		default: 'arcade',
		arcade: {
			//gravity: { y: 200 },
			debug: true
		},
		checkCollision: {
			up: true,
			down: true,
			left: true,
			right: true
		}
	},
	title: "PVLI Ordinaria 25/26",
	version: "1.0.0",
	transparent: false
};

new Phaser.Game(config);

/* EXPLICACIONES

ESTRUCTURA GENERAL
-------------------
game.js es el punto de entrada de todo el proyecto — el único archivo que index.html carga
directamente (<script src="./src/game.js" type="module">). Su única responsabilidad es definir
el objeto de configuración del juego y arrancar la instancia de Phaser.Game con él. No contiene
ninguna lógica de juego propia.

Imports
---------
Los tres imports traen las clases de cada escena (cada una con su propio export default en su
archivo). type="module" en el <script> del HTML es lo que habilita la sintaxis import/export de
ES Modules en el navegador sin necesidad de un bundler (Webpack, Vite...) — el navegador
resuelve las rutas relativas ("./GameScene.js") directamente.

config — propiedad por propiedad
------------------------------------
- type: Phaser.AUTO: deja que Phaser decida automáticamente si renderizar con WebGL (más
  rápido, usa la GPU) o, si el navegador no lo soporta, caer a Canvas2D como alternativa.
- parent: 'juego': el id del <div> del HTML donde Phaser inyecta el <canvas> que genera. Tiene
  que coincidir exactamente con el id definido en index.html (<div id='juego'></div>).
- pixelArt: true: desactiva el suavizado (antialiasing/interpolación) al escalar las imágenes,
  imprescindible para que los sprites de baja resolución (32x32) se vean nítidos y "pixelados"
  a propósito en vez de borrosos al hacer zoom o escalar el canvas.
- scale: controla cómo se adapta el canvas al contenedor. width/height (500x300) son las
  dimensiones lógicas del juego (el sistema de coordenadas que usamos en todo el código: x de
  0 a 500, y de 0 a 300). mode: Phaser.Scale.FIT hace que el canvas se reescale visualmente para
  caber en su contenedor manteniendo la proporción, sin deformar la imagen. autoCenter:
  CENTER_HORIZONTALLY centra el canvas horizontalmente dentro de su contenedor. zoom: 1 significa
  sin zoom adicional más allá del que ya aplique el modo FIT.
- scene: [MenuScene, GameScene, GameOverScene]: el array de TODAS las escenas que existen en el
  juego. Importante: Phaser arranca automáticamente la PRIMERA del array al cargar el juego (de
  ahí que MenuScene vaya primero) — las demás quedan "registradas" pero inactivas hasta que algo
  llame a scene.start('SuClave') sobre ellas.
- physics.default: 'arcade': el motor de físicas a usar en todo el juego (Arcade Physics es el
  más simple y rápido de los que ofrece Phaser, basado en cajas alineadas a los ejes — AABB —
  sin rotación de los cuerpos físicos, suficiente para este tipo de juego top-down).
- physics.arcade.gravity: comentada a propósito — este es un juego de vista cenital (como el
  TRON original), no de plataformas, así que no se quiere que los objetos caigan solos. Se deja
  la línea comentada como referencia/plantilla en vez de borrarla del todo.
- physics.arcade.debug: true: dibuja overlays visuales (las cajas verdes de colisión, vectores
  de velocidad, etc.) sobre el canvas, útil durante el desarrollo para verificar visualmente que
  los bodies físicos están donde deberían. Se dejaría en false en una build "de producción" para
  el jugador final, aunque para la entrega del examen no es estrictamente necesario tocarlo.
- physics.arcade.checkCollision: habilita la detección de colisión en los 4 bordes del MUNDO
  físico global (no de las barreras, que son objetos aparte) — como el juego no usa
  setCollideWorldBounds() en ningún personaje, esta configuración en la práctica no tiene efecto
  visible aquí (la contención del campo de juego la hacen las barreras, no los bordes del
  canvas), pero se deja tal cual venía en la plantilla del profesor.
- title / version: metadatos puramente informativos del juego, no afectan a la lógica ni a la
  física, Phaser los usa solo para mostrarlos en la consola del navegador al arrancar.
- transparent: false: el canvas tiene un fondo opaco (no se ve nada "a través" de él) en vez de
  dejar transparentar el fondo de la página HTML por debajo.

new Phaser.Game(config)
---------------------------
La línea que realmente arranca todo: crea la instancia del motor con la configuración anterior,
inyecta el canvas en el DOM, inicializa el bucle de render/update interno, y arranca
automáticamente la primera escena del array. A partir de aquí, todo el control pasa a las
propias escenas (MenuScene.create(), etc.) — este archivo no vuelve a ejecutar nada más.

*/
