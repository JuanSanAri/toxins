export default class MenuScene extends Phaser.Scene {
	constructor() {
		super({ key: 'MenuScene' });
	}

	create() {
		// Título TRON en blanco
		this.add.text(250, 90, 'TRON', {
			fontFamily: 'arcade',
			fontSize: '48px',
			color: '#ffffff'
		}).setOrigin(0.5);

		// Subtítulo en verde, más pequeño
		this.add.text(250, 140, 'Deadly Discs', {
			fontFamily: 'arcade',
			fontSize: '24px',
			color: '#00ff00'
		}).setOrigin(0.5);

		// Texto parpadeante
		let pressText = this.add.text(250, 220, 'Pulsa ESPACIO para continuar', {
			fontFamily: 'arcade',
			fontSize: '16px',
			color: '#ffffff'
		}).setOrigin(0.5);

		this.tweens.add({
			targets: pressText,
			alpha: 0,
			duration: 500,
			yoyo: true,
			repeat: -1
		});

		// Al pulsar espacio, una sola vez, pasamos a la escena de juego
		this.input.keyboard.once('keydown-SPACE', () => {
			this.scene.start('GameScene');
		});
	}
}

/* EXPLICACIONES

ESTRUCTURA GENERAL
-------------------
MenuScene es la PRIMERA escena del array scene:[] en game.js, así que Phaser la arranca
automáticamente al cargar el juego. No tiene preload() porque no usa ninguna imagen ni sonido
propio: el único recurso que necesita es la fuente 'arcade', que no se carga vía Phaser sino
que ya viene cargada por el navegador desde css/game.css (@font-face). Por eso podemos usarla
en fontFamily sin haber hecho this.load.algo() antes.

create()
--------
Se ejecuta una sola vez al entrar en la escena. Construye 3 elementos de texto y la lógica de
salto a la siguiente escena:

1. "TRON": this.add.text(x, y, contenido, estilo) crea un GameObject de tipo Text en la
   posición (250, 90). 250 es el centro horizontal del canvas (500px de ancho / 2).
   .setOrigin(0.5) recoloca el "punto de anclaje" del texto en su propio centro (por defecto
   el ancla está en la esquina superior izquierda), así el texto queda realmente centrado sobre
   el punto x=250 en vez de empezar a dibujarse ahí.

2. "Deadly Discs": mismo patrón, pero más abajo (y=140), fuente más pequeña (24px vs 48px) y en
   verde (#00ff00), tal y como pide el enunciado.

3. Texto de "pulsa espacio": se guarda en una variable (pressText) porque lo necesitamos
   referenciar después para animarlo con un tween.

EL PARPADEO (tween)
--------------------
this.tweens.add({...}) crea una animación de propiedades sobre el objeto indicado en targets.
Aquí animamos su 'alpha' (opacidad, de 1 a 0) durante 500ms. yoyo:true hace que, al llegar a 0,
la animación vuelva automáticamente a 1 (sin esto solo se desvanecería una vez y se quedaría
invisible). repeat:-1 significa "repetir infinitamente" (cualquier número >=0 sería "repetir esa
cantidad de veces además de la primera pasada"; -1 es el valor especial para "para siempre").
Resultado: el texto se desvanece y reaparece en bucle sin que tengamos que tocar nada más.

EL SALTO DE ESCENA
-------------------
this.input.keyboard.once('keydown-SPACE', callback) registra un listener de teclado que se
dispara UNA SOLA VEZ la primera vez que se detecta que la tecla espacio pasa a estado
"pulsada". Usamos .once() en vez de .on() precisamente porque solo queremos que el salto de
escena ocurra una vez; si usáramos .on() y por error el listener no se limpiara al cambiar de
escena, podría quedar resonando en el fondo. Dentro del callback, this.scene.start('GameScene')
detiene MenuScene y arranca GameScene (la clave 'GameScene' debe coincidir exactamente, letra
por letra, con el key que GameScene define en su propio constructor vía super({key:...})).

POR QUÉ NO HAY UPDATE()
-------------------------
Esta escena es totalmente pasiva: no hay nada que comprobar frame a frame de forma manual,
el parpadeo lo gestiona el sistema de tweens internamente y el salto de escena lo gestiona el
sistema de eventos de teclado; ninguno de los dos necesita que nosotros sondeemos su estado en
un bucle update(). Es la prueba de que no toda escena de Phaser necesita un update().

*/
