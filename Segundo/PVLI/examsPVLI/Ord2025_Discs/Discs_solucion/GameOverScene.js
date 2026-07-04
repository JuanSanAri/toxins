export default class GameOverScene extends Phaser.Scene {
	constructor() {
		super({ key: 'GameOverScene' });
	}

	init(data) {
		this.finalScore = data.score || 0;
	}

	create() {
		this.add.text(250, 120, 'GAME OVER', {
			fontFamily: 'arcade',
			fontSize: '32px',
			color: '#ffffff'
		}).setOrigin(0.5);

		this.add.text(250, 160, 'Puntuación: ' + this.finalScore, {
			fontSize: '16px',
			color: '#ffffff'
		}).setOrigin(0.5);

		this.time.delayedCall(3000, () => {
			this.scene.start('MenuScene');
		});
	}
}

/* EXPLICACIONES

ESTRUCTURA GENERAL
-------------------
GameOverScene es la tercera y última escena del juego (Menu -> Game -> GameOver -> Menu otra vez,
en bucle). Su única responsabilidad es: mostrar el resultado final 3 segundos, y volver al menú.
No tiene preload() porque no carga ningún asset nuevo (el texto usa la fuente 'arcade', que ya
está cargada globalmente desde el CSS del proyecto, y no usa ninguna imagen propia).

constructor()
-------------
super({ key: 'GameOverScene' }) registra esta escena en el sistema de Phaser bajo la clave
'GameOverScene'. Esa clave (el string) es la que se usa en cualquier otro punto del código para
arrancarla, p.ej. this.scene.start('GameOverScene', {...}) desde Player.js. El nombre de la
clase JS (GameOverScene) es independiente del key; podrían no coincidir y funcionaría igual,
pero por convención y legibilidad los mantenemos iguales.

init(data)
----------
Método especial de Phaser que se ejecuta automáticamente justo ANTES de preload()/create(),
cada vez que la escena arranca con scene.start(). Recibe como argumento el objeto de datos que
se le pasó en la llamada (en este caso, { score: this.scene.score } desde Player.loseLife()).
Aquí simplemente lo guardamos en una propiedad de la instancia (this.finalScore) para poder
usarlo luego en create(). El "|| 0" es una salvaguarda: si por lo que sea alguien arranca esta
escena sin pasarle datos (p.ej. probándola suelta durante el desarrollo), data.score sería
undefined, y undefined || 0 cae a 0 en vez de mostrar "Puntuación: undefined" en pantalla.

create()
--------
Se ejecuta una vez, justo después de init(). Aquí montamos la interfaz:

1. Texto "GAME OVER" en blanco, fuente 'arcade', centrado en x=250 (mitad del canvas de 500px
   de ancho) mediante .setOrigin(0.5) (centra el ancla del texto en su propio punto medio, no
   solo en X sino también en Y por defecto, ya que .setOrigin(0.5) sin segundo argumento aplica
   0.5 a ambos ejes).

2. Texto con la puntuación final, concatenando el string fijo 'Puntuación: ' con el valor
   numérico this.finalScore (JS convierte el número a string automáticamente al concatenarlo
   con +). Fuente por defecto de Phaser (no 'arcade'), más pequeña, también centrada.

3. this.time.delayedCall(3000, callback): el temporizador propio de Phaser (no es el setTimeout
   nativo de JS) que ejecuta el callback una sola vez pasados 3000ms. Dentro, llamamos a
   this.scene.start('MenuScene'), lo que destruye esta escena (GameOverScene) y arranca de
   nuevo MenuScene desde cero. Como GameScene se destruyó ya antes (al entrar aquí), todo el
   estado de la partida anterior (vidas, oleada, enemigos, score...) desaparece sin que tengamos
   que limpiarlo a mano: Phaser se encarga de destruir cada escena y sus objetos/timers/tweens
   asociados en su shutdown().

POR QUÉ ESTE ORDEN
------------------
constructor (registro de la escena, una sola vez al cargar el juego) -> init (recibe datos
frescos cada vez que se entra a la escena) -> create (construye la interfaz visual usando esos
datos). Es el ciclo de vida estándar de cualquier Phaser.Scene cuando necesitas pasar
información entre escenas.

*/
