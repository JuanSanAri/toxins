import Disc from "./Disc.js";

export default class Player extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y) {
		super(scene, x, y, 'player');

		// Añadir manualmente el player con física (collider)
		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.setOrigin(0.5);

		// Cuerpo 2/3 del tamaño del sprite
		this.setSize(this.width * 2 / 3, this.height * 2 / 3);
		this.setOffset(this.width / 6, this.height / 6);

		this.speed = 80; // speed, multiplicará al vector de mov.
		this.facing = 'right'; // dirección por defecto

		// Teclas que tomará en cuenta la clase Player
		this.keys = scene.input.keyboard.addKeys('W,A,S,D,SPACE');
		this.disc = null;

		// Sistema vidas y color
		this.lives = 3;
		this.invulnerable = false;
		this.canMove = true;
		this.updateColor();

		// Anim
		this.play('player_idle');
	}

	update() {
		if (!this.canMove) return;

		let dx = 0;
		let dy = 0;

		// Si la tecla W se mantiene pulsada, o no
		if (this.keys.W.isDown) dy = -1;
		else if (this.keys.S.isDown) dy = 1;

		// Lo mismo con el eje X pero cambiando
		// el valor de facing para el flip
		if (this.keys.A.isDown) {
			dx = -1;
			this.facing = 'left';
		} else if (this.keys.D.isDown) {
			dx = 1;
			this.facing = 'right';
		}

		// Vector normalizado para la velocidad diagonal
		let velocity = new Phaser.Math.Vector2(dx, dy);
		velocity.normalize().scale(this.speed);
		// Vector de movimiento
		this.setVelocity(velocity.x, velocity.y);

		if (dx !== 0 || dy !== 0) {
			this.play('player_run', true);
		} else {
			this.play('player_idle', true);
		}

		// setFlipX es un método de Phaser (al igual que setVelocity)
		this.setFlipX(this.facing === 'left');

		// Disco
		if (Phaser.Input.Keyboard.JustDown(this.keys.SPACE)) {
			this.tryThrowDisc();
		}
		if (this.disc) {
			this.disc.update();
		}
	}

	tryThrowDisc() {
		if (!this.disc) {
			let direction = this.getThrowDirection();
			this.disc = new Disc(this.scene, this.x, this.y, direction, this);

			this.scene.physics.add.collider(this.disc, this.scene.barriers, () => {
				this.disc.bounce();
			});

			this.scene.physics.add.overlap(this.disc, this.scene.enemies, (discObj, enemyObj) => {
				if (this.disc.state === 'thrown') {
					enemyObj.loseLife();
				}
			});

		} else if (this.disc.state === 'thrown') {
			this.disc.forceReturn();
		}
	}

	getThrowDirection() {
		let vx = this.body.velocity.x;
		let vy = this.body.velocity.y;

		if (vx === 0 && vy === 0) {
			return { x: 0, y: -1 }; // parado -> siempre hacia arriba
		}

		let length = Math.sqrt(vx * vx + vy * vy);
		return { x: vx / length, y: vy / length };
	}

	updateColor() {
		const colors = { 3: 0x3399ff, 2: 0xff66cc, 1: 0xffff66 };
		this.setTint(colors[this.lives]);
	}

	loseLife() {
		if (this.invulnerable || this.lives <= 0) return;

		this.lives--;
		this.scene.sound.play('playerHit');

		if (this.lives <= 0) {
			this.setTint(0xffffff);
			this.setVelocity(0, 0);
			this.play('player_idle'); // corta la animación de correr si la había
			this.canMove = false; // pierde la capacidad de moverse

			// Muerte: sonido + transición a GameOverScene
			let looseSound = this.scene.sound.add('loose');
			looseSound.play();
			this.scene.time.delayedCall(2000, () => {
				looseSound.stop();
				this.scene.scene.start('GameOverScene', { score: this.scene.score });
			});
			return;
		}

		this.updateColor();
		this.invulnerable = true;
		this.scene.tweens.add({
			targets: this,
			alpha: 0.3,
			duration: 100,
			yoyo: true,
			repeat: 9 // 10 ciclos × 200ms ≈ 2 segundos
		});
		this.scene.time.delayedCall(2000, () => {
			this.invulnerable = false;
			this.setAlpha(1); // por si el tween no terminó en opacidad completa
		});
	}
}

/* EXPLICACIONES

ESTRUCTURA GENERAL
-------------------
Player extiende Phaser.Physics.Arcade.Sprite, lo que le da de serie todos los métodos de
movimiento/física (setVelocity, body, etc.) y de representación visual (setFlipX, setTint,
play() para animaciones...). Es el GameObject controlado por teclado, y es quien "posee" al
disco que lanza (this.disc), igual que cada Enemy posee el suyo.

REDUNDANCIA CORREGIDA: en una versión anterior de este archivo existían DOS métodos
tryThrowDisc() definidos en la misma clase. En JavaScript, cuando repites el nombre de un
método dentro de una clase, el SEGUNDO sobrescribe completamente al primero (no se combinan ni
da error) — el primero queda como código muerto, nunca se ejecuta. Aquí ya solo queda la
versión correcta y completa.

constructor(scene, x, y)
-------------------------
- super(scene, x, y, 'player'): inicializa el Sprite con la textura 'player' (cargada como
  spritesheet en GameScene.preload(), con frames de 32x32, necesarios para las animaciones).
- scene.add.existing(this) / scene.physics.add.existing(this): igual que en Disc, registran la
  instancia en la escena y le dan cuerpo físico. Extender la clase NO lo hace automático.
- setSize(...) / setOffset(...): el body de colisión (la caja invisible que usa la física) se
  reduce a 2/3 del tamaño del sprite visual (pedido explícitamente por el enunciado, para que
  esquivar discos sea un poco más permisivo de lo que parece a simple vista). setOffset
  recentra esa caja más pequeña dentro del sprite (si no, quedaría pegada a la esquina superior
  izquierda en vez de centrada).
- this.speed = 80: velocidad de desplazamiento en píxeles/segundo (la usa Arcade Physics
  directamente al hacer setVelocity).
- this.facing: variable propia (no la entiende Phaser, es nuestra) para recordar hacia qué lado
  miraba el jugador la última vez que pulsó A o D. Se usa solo para decidir el flip del sprite.
- this.keys = scene.input.keyboard.addKeys('W,A,S,D,SPACE'): registra las 5 teclas de una vez,
  devolviendo un objeto con cada una accesible por su letra (this.keys.W.isDown, etc.).
- this.disc = null: de momento no hay ningún disco lanzado.
- this.lives, this.invulnerable, this.canMove: estado del sistema de vidas (ver loseLife()).
  updateColor() se llama ya en el constructor para que el jugador aparezca con su color
  correspondiente a 3 vidas (azul) desde el primer frame.
- this.play('player_idle'): arranca la animación de quieto nada más aparecer.

update()
--------
Se ejecuta una vez por frame, llamado explícitamente desde GameScene.update() (Phaser nunca
llama solo al update() de un GameObject custom, solo al de la Scene).

1. if (!this.canMove) return: si el jugador se quedó sin vidas, cortamos aquí todo el método:
   ni lee teclado, ni cambia animación, ni actualiza el disco. Por eso en loseLife() también
   hace falta poner la velocidad a 0 manualmente — canMove solo bloquea LECTURA de input nuevo,
   no congela por sí solo la velocidad que ya llevaba el cuerpo físico en el momento de morir.

2. Lectura de W/A/S/D: dx/dy van de -1 a 1 según qué teclas estén pulsadas AHORA MISMO
   (.isDown, no JustDown, porque queremos movimiento continuo mientras se mantenga pulsada).
   Dentro del bloque de A/D actualizamos this.facing SOLO cuando A o D están activas; si no se
   toca ninguna de las dos, facing conserva su último valor (así, moverse solo en vertical no
   hace que el personaje "olvide" hacia dónde miraba).

3. Vector normalizado: nuevo Vector2(dx, dy), .normalize() lo reduce a longitud 1 conservando
   la dirección, y .scale(this.speed) lo estira hasta la velocidad deseada. Esto es necesario
   para que moverse en diagonal (dx=1, dy=-1, por ejemplo) no sea más rápido que moverse en
   línea recta (sin normalizar, la magnitud de un vector (1,-1) sería raíz de 2, ~1.41 veces
   más rápido por el teorema de Pitágoras).

4. setVelocity(velocity.x, velocity.y): aplica el vector calculado al body físico; a partir de
   aquí Arcade Physics mueve al jugador solo cada frame, no hace falta tocar this.x/this.y a
   mano.

5. Animación: si hay movimiento (dx!=0 o dy!=0) reproduce 'player_run', si no 'player_idle'. El
   segundo argumento true en .play(key, true) significa "ignora esta llamada si esa animación
   ya se está reproduciendo" — sin él, se reiniciaría desde el frame 0 cada uno de los 60 frames
   por segundo, dando un parpadeo feo en vez de una animación fluida.

6. setFlipX(this.facing === 'left'): espeja horizontalmente el sprite si toca mirar a la
   izquierda. setFlipX es un método heredado de Phaser.GameObjects.Sprite (no es físico, es
   puramente visual).

7. Bloque del disco: JustDown(SPACE) detecta el frame EXACTO en que se pulsó espacio (a
   diferencia de isDown, que sería true mientras se mantenga pulsada, lanzando discos sin
   parar). Si se cumple, llama a tryThrowDisc(). Independientemente de eso, si ya existe un
   disco activo (this.disc no es null), se le llama a su propio update() cada frame — necesario
   para que su lógica de "volver persiguiendo al dueño" funcione (ver Disc.js).

tryThrowDisc()
---------------
- Si no hay disco activo (this.disc es null): calcula la dirección de lanzamiento
  (getThrowDirection()), crea el Disc, y registra DOS detectores de colisión propios de ESTE
  disco concreto:
    a) un collider contra this.scene.barriers que llama a this.disc.bounce() al chocar.
    b) un overlap contra this.scene.enemies (el grupo) que, solo si el disco sigue en estado
       'thrown' (no de vuelta), resta una vida al enemigo concreto que tocó (enemyObj, el
       segundo parámetro que Phaser pasa automáticamente al callback de overlap, identificando
       a CUÁL de los miembros del grupo se solapó).
  Estos colliders/overlaps se registran aquí, en el momento de crear el disco, y no una sola
  vez en GameScene.create(), porque el disco no existe hasta que se lanza — no se puede
  registrar una colisión sobre un objeto que todavía no existe.

- Si ya hay un disco Y sigue en estado 'thrown' (todavía no ha chocado con nada): pulsar espacio
  otra vez fuerza la vuelta anticipada (this.disc.forceReturn()), tal y como pide el enunciado.
  Si el disco ya estaba 'returning', este bloque no hace nada (no hay ninguna acción definida
  para "forzar la vuelta de un disco que ya está volviendo").

getThrowDirection()
---------------------
Lee la velocidad ACTUAL del cuerpo físico del jugador (this.body.velocity), no su input de
teclado — son conceptualmente lo mismo en este caso porque el body.velocity ya se ha calculado
arriba en update() antes de llegar aquí, pero usar la velocidad real es más robusto que leer
this.keys otra vez. Si está completamente parado (vx=0 y vy=0), el enunciado pide que el disco
salga siempre hacia arriba, de ahí el caso especial { x: 0, y: -1 } (en Phaser, "arriba" es Y
negativo, ya que el eje Y crece hacia ABAJO en pantalla, al contrario que en matemáticas). Si
hay movimiento, se normaliza el vector de velocidad para obtener solo la dirección (longitud 1).

updateColor()
--------------
Diccionario sencillo {vidas: colorHex} y this.setTint(color) aplica un tinte de color sobre el
sprite (multiplicando sus píxeles por ese color, por eso funciona igual de bien sobre cualquier
frame de la animación, no hace falta tener sprites pre-coloreados para cada caso).

loseLife()
-----------
Guarda de entrada: if (this.invulnerable || this.lives <= 0) return — evita procesar daño si
el jugador está en su ventana de invulnerabilidad post-golpe, o si ya estaba a 0 vidas (evita
errores raros si por overlaps simultáneos se llamara dos veces seguidas el mismo frame).

Resta una vida y reproduce el sonido de golpe. Si llega a 0:
- Tinte blanco, velocidad a 0 en seco, animación forzada a 'player_idle' (corta cualquier
  animación de correr que estuviera en marcha), y canMove = false (bloquea el input desde el
  próximo update()).
- Reproduce el sonido de derrota (this.scene.sound.add('loose') crea una instancia de sonido
  controlable, a diferencia de this.scene.sound.play('loose') que la dispara y la "olvida"; la
  necesitamos controlable porque más adelante queremos poder pararla a mano con .stop()).
- this.scene.time.delayedCall(2000, callback): pasados 2 segundos exactos, para el sonido y
  lanza this.scene.scene.start('GameOverScene', { score: this.scene.score }). Aquí hay DOS
  ".scene" distintos y es fácil confundirse: this.scene (el primero) es la instancia de
  GameScene a la que pertenece este Player; .scene (el segundo, sobre esa instancia) es el
  Scene Plugin que toda escena de Phaser lleva integrado para gestionar transiciones
  (start/stop/pause...). this.scene.score es la puntuación acumulada, leída directamente de la
  escena dueña.

Si en cambio sigue con vidas > 0: actualiza el color (updateColor()), activa invulnerable, y
lanza un tween de parpadeo de alpha (10 ciclos de 200ms = 2000ms, coincidiendo exactamente con
la ventana de invulnerabilidad). El delayedCall final desactiva la invulnerabilidad Y fuerza
setAlpha(1) como salvaguarda, por si el tween y el timer no terminaran en el milisegundo exacto
(podría quedar el sprite a medio parpadeo, semitransparente para siempre, sin este reset).

*/