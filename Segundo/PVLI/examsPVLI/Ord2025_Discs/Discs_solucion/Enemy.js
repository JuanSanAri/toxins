import Disc from "./Disc.js";

export default class Enemy extends Phaser.Physics.Arcade.Sprite {
	constructor(scene, x, y, speed = 50, lives = 1) {
		super(scene, x, y, 'enemy');

		scene.add.existing(this);
		scene.physics.add.existing(this);

		this.setOrigin(0.5);
		this.setSize(this.width * 2 / 3, this.height * 2 / 3);
		this.setOffset(this.width / 6, this.height / 6);

		this.speed = speed;
		this.canBounce = true; // flag de cooldown para evitar dobles rebotes en la misma colisión

		// Vidas y color
		this.lives = lives;
		this.invulnerable = false;
		// verde si 2 vidas, naranja si 1
		this.setTint(this.lives === 2 ? 0x33cc33 : 0xffa500);

		// Disco
		this.disc = null;
		this.scheduleNextThrow();

		// Anim
		this.play('enemy_run');

		// IMPORTANTE: este constructor NO llama a setRandomDirection().
		// Se llama desde fuera, en GameScene.spawnWave(), justo DESPUÉS de
		// añadir el enemigo al grupo physics.add.group(). Si se llamara aquí,
		// el grupo resetearía la velocidad a 0 al hacer this.enemies.add(enemy)
		// (ver explicación detallada abajo).
	}

	setRandomDirection() {
		let angle = Phaser.Math.Between(0, 359);
		let velocity = this.scene.physics.velocityFromAngle(angle, this.speed);
		this.direction = { x: velocity.x, y: velocity.y }; // copia propia, intocable por Arcade
		this.setVelocity(this.direction.x, this.direction.y);
	}

	reverseDirection() {
		// si está en cooldown, ignoramos el choque
		if (!this.canBounce) return;

		this.direction.x *= -1;
		this.direction.y *= -1;
		this.setVelocity(this.direction.x, this.direction.y); // sobreescribimos lo que Arcade haya tocado

		this.canBounce = false;
		this.scene.time.delayedCall(200, () => {
			this.canBounce = true;
		});
	}

	loseLife() {
		if (this.invulnerable || this.lives <= 0) return;

		this.lives--;

		if (this.lives <= 0) {
			this.scene.sound.play('enemyDeath');
			this.scene.addScore(150);
			if (this.disc) {
				this.disc.destroy();
				this.disc = null;
			}
			this.destroy();
			return;
		}

		// Si sigue vivo NO cambia de color (lo pide así el enunciado),
		// simplemente pilla invulnerabilidad durante 1s
		this.invulnerable = true;
		// No lo pide el enunciado pero da feedback visual del golpe
		this.scene.tweens.add({
			targets: this,
			alpha: 0.3,
			duration: 100,
			yoyo: true,
			repeat: 4 // 5 ciclos de ~200ms ≈ 1 segundo
		});
		this.scene.time.delayedCall(1000, () => {
			this.invulnerable = false;
			this.setAlpha(1);
		});
	}

	scheduleNextThrow() {
		let delay = Phaser.Math.Between(1500, 3000); // entre 1.5 y 3 segundos
		this.throwTimer = this.scene.time.delayedCall(delay, () => {
			this.tryThrowDisc();
			this.scheduleNextThrow(); // se reprograma a sí mismo
		});
	}

	tryThrowDisc() {
		if (this.disc) return; // ya tiene uno fuera, no lanza otro

		let player = this.scene.player;
		let dx = player.x - this.x;
		let dy = player.y - this.y;
		let length = Math.sqrt(dx * dx + dy * dy) || 1;
		let direction = { x: dx / length, y: dy / length };

		this.disc = new Disc(this.scene, this.x, this.y, direction, this, this.speed + 100);
		this.disc.setTint(0xff0000);

		this.scene.physics.add.collider(this.disc, this.scene.barriers, () => {
			this.disc.bounce();
		});

		this.scene.physics.add.overlap(this.disc, this.scene.player, () => {
			if (this.disc.state === 'thrown') {
				this.scene.player.loseLife();
			}
		});
	}

	update() {
		if (this.disc) {
			this.disc.update();
		}

		// Espeja el sprite según el sentido horizontal actual del movimiento
		if (this.body.velocity.x !== 0) {
			this.setFlipX(this.body.velocity.x > 0);
		}
	}

	destroy(fromScene) {
		if (this.throwTimer) this.throwTimer.remove(); // evita que siga intentando lanzar tras morir
		if (this.disc) this.disc.destroy();
		super.destroy(fromScene);
	}
}

/* EXPLICACIONES

ESTRUCTURA GENERAL
-------------------
Enemy extiende Phaser.Physics.Arcade.Sprite, igual que Player. Cada instancia se gestiona desde
GameScene a través de un Phaser.Physics.Arcade.Group (this.enemies en GameScene), creado y
poblado en spawnWave(). El enemigo combina tres comportamientos independientes que conviven en
la misma clase: patrullaje (rebote en línea recta), vidas/daño, y lanzamiento de discos hacia
el jugador.

constructor(scene, x, y, speed, lives)
------------------------------------------
Idéntico patrón de inicialización física que Player (add.existing + physics.add.existing +
body reducido a 2/3 con offset). this.canBounce arranca en true (puede rebotar libremente desde
el primer choque). this.lives se inicializa con el valor recibido (1 o 2 según si esta oleada es
"especial", decidido en GameScene.spawnWave()), y el tinte de color se asigna una sola vez aquí,
en función de las vidas iniciales — a diferencia del jugador, el enemigo NO vuelve a llamar a
ningún "updateColor()" cuando pierde una vida, porque el enunciado pide explícitamente que su
color NO cambie al perder vidas (solo el jugador cambia de color progresivamente).

this.disc = null seguido de this.scheduleNextThrow() arranca el temporizador propio de este
enemigo para empezar a lanzar discos sin que nadie más tenga que llamarlo.

POR QUÉ NO SE LLAMA A setRandomDirection() AQUÍ DENTRO (la "trampa" más sutil del proyecto)
------------------------------------------------------------------------------------------------
Phaser.Physics.Arcade.Group (creado en GameScene con this.physics.add.group()) aplica sus
valores de configuración por defecto a CUALQUIER objeto que se le añada con group.add(objeto),
incluida la velocidad — incluso si ese objeto ya tenía una velocidad propia puesta de antes.
Como el grupo de enemigos se crea sin configuración explícita, sus defaults son velocidad 0.
Así que si this.setRandomDirection() se llamara aquí, dentro del constructor, la secuencia real
en GameScene.spawnWave() sería:
  1. new Enemy(...) -> setRandomDirection() pone una velocidad real.
  2. this.enemies.add(enemy) -> el grupo RESETEA esa velocidad a 0.
Resultado: el enemigo se queda completamente quieto, aunque toda la lógica de física esté bien
escrita. La solución fue mover la llamada a setRandomDirection() a GameScene.spawnWave(),
justo DESPUÉS de this.enemies.add(enemy), para que la velocidad se aplique cuando ya no hay
nadie que la vaya a resetear después.

setRandomDirection()
----------------------
Elige un ángulo aleatorio entre 0 y 359 grados (Phaser.Math.Between es inclusivo en ambos
extremos) y usa this.scene.physics.velocityFromAngle(angulo, velocidad) — un helper de Phaser
que hace la trigonometría (seno/coseno) por nosotros y devuelve directamente un vector {x, y}
con esa velocidad en esa dirección. Guardamos una COPIA propia en this.direction, separada del
body.velocity real, porque Arcade Physics puede mutar body.velocity por su cuenta durante la
resolución de colisiones (con bounce=0, que es el default, anula a 0 la componente de velocidad
que choca contra un objeto inmóvil ANTES de que nuestro código de colisión llegue a ejecutarse).
Si leyéramos this.body.velocity para calcular el rebote, leeríamos un valor ya corrompido.
this.direction es nuestra "fuente de la verdad", intocable por el motor de física.

reverseDirection()
---------------------
Se llama desde GameScene.onEnemyHitBarrier(), registrado como callback del collider entre el
grupo de enemigos y el grupo de barreras. La guarda if (!this.canBounce) return implementa un
"cooldown" de 200ms (ver más abajo) para evitar un bug de temblor: sin cooldown, mientras el
cuerpo del enemigo sigue técnicamente solapado con la barrera durante uno o varios frames
seguidos justo después del primer rebote, Phaser seguiría disparando el evento de colisión
repetidamente, invirtiendo la dirección una y otra vez en bucle (el enemigo "vibra" pegado a la
pared en vez de alejarse limpiamente).

Cuando sí puede rebotar: invierte AMBOS ejes de this.direction (x*=-1, y*=-1) — esto hace que el
enemigo retroceda exactamente por la misma línea por la que vino (un rebote "de billar" que solo
invirtiera el eje perpendicular a la pared chocada habría producido una trayectoria distinta,
no la misma línea de ida y vuelta que muestra la Figura 10 del enunciado). Se reaplica con
setVelocity(), sobreescribiendo cualquier cosa que Arcade Physics le hubiera tocado durante la
resolución de la colisión de este mismo frame. Luego se desactiva canBounce durante 200ms con
this.scene.time.delayedCall(), tiempo de sobra para que el cuerpo se separe físicamente de la
barrera antes de que se le permita volver a rebotar.

loseLife()
-----------
Mismo patrón de guarda que en Player (invulnerable o ya muerto -> no hacer nada). Si llega a
0 vidas: suena 'enemyDeath', se suman 150 puntos a la escena (this.scene.addScore(150)), y si
el enemigo tenía un disco propio activo en el momento de morir, se destruye también
explícitamente (this.disc.destroy()) — si no se hiciera, ese disco quedaría volando "huérfano"
en la escena, sin nadie capaz de recogerlo nunca (su owner ya no existiría), aunque en la
práctica tampoco sería grave porque al cambiar de escena se limpiaría todo igualmente.
this.destroy() (el propio, ver más abajo) hace el resto de la limpieza.

Si sobrevive (tenía 2 vidas, ahora le queda 1): activa invulnerable durante 1 segundo y lanza un
tween de parpadeo de alpha como feedback visual (no lo pide el enunciado, es un añadido nuestro
para que se note el golpe). Igual que en Player, el delayedCall final fuerza setAlpha(1) como
salvaguarda por si el tween no terminara exactamente en opacidad completa.

scheduleNextThrow()
----------------------
Patrón de "temporizador que se reprograma a sí mismo": en vez de un intervalo fijo repetido
(addEvent con loop:true), cada vez que se dispara, además de intentar lanzar un disco
(tryThrowDisc()), vuelve a llamarse a sí misma para programar el SIGUIENTE lanzamiento con un
nuevo delay aleatorio (entre 1.5 y 3 segundos). Esto da variedad: cada enemigo lanza con un
ritmo ligeramente distinto e impredecible, en vez de todos sincronizados al mismo compás.

tryThrowDisc()
----------------
Si el enemigo ya tiene un disco activo (this.disc no es null), no hace nada — los personajes
solo pueden tener un disco fuera a la vez. Si no, calcula la dirección hacia la posición ACTUAL
del jugador en este instante concreto (dx, dy, normalizado) — a diferencia del disco que vuelve
(que persigue al dueño cada frame), este cálculo se hace UNA SOLA VEZ en el momento del
lanzamiento, tal y como exige el enunciado ("irá en la dirección donde se encontraba el jugador
en el momento del lanzamiento", no donde esté después). El "|| 1" en el divisor de length es una
protección por si, en un caso extremadamente raro, el enemigo y el jugador estuvieran exactamente
en el mismo píxel (length=0), evitando una división por cero que daría NaN.

Crea el Disc pasándole this.speed + 100 como velocidad (más rápido que el propio enemigo, para
que los discos enemigos supongan una amenaza real incluso si el jugador esquiva al enemigo
mismo), y le aplica un tinte rojo fijo (this.disc.setTint(0xff0000)) para diferenciarlo
visualmente del disco del jugador (que no lleva tinte). El enunciado pide que el disco herede el
color de quien lo lanza de forma dinámica; aquí se simplificó a un rojo fijo para los discos
enemigos, decisión consciente para priorizar tiempo en otras partes del examen.

Registra su propio collider (rebote contra barreras) y overlap (daño al jugador si sigue en
estado 'thrown') exactamente con el mismo patrón que el disco del jugador en Player.js.

update()
----------
Llamado desde GameScene.update() mediante this.enemies.getChildren().forEach(e => e.update()).
Dos responsabilidades: (1) si tiene un disco activo, llamar a su update() (necesario para que la
lógica de persecución de vuelta del disco, ver Disc.js, funcione — sin esta línea, el disco del
enemigo se quedaría parado nada más rebotar, exactamente el mismo bug que tuvimos con el
jugador antes de añadir esta llamada); (2) actualizar el flip horizontal según el signo de la
velocidad X actual del body. No hace falta guardar una variable "facing" como en Player porque
el enemigo no tiene "última dirección pulsada" que recordar — su dirección siempre está
disponible en tiempo real en su propia velocidad.

destroy(fromScene)
---------------------
Sobrescribe el destroy() heredado de Phaser.GameObjects.GameObject. Antes de dejar que Phaser
haga la destrucción real (super.destroy(fromScene)), limpiamos dos referencias propias:
- this.throwTimer.remove(): cancela el temporizador de scheduleNextThrow(). Sin esto, el timer
  seguiría disparándose más adelante sobre un enemigo ya destruido, intentando leer this.x o
  this.scene de un objeto que ya no existe del todo, lanzando un error en consola.
- this.disc.destroy(): si tenía un disco activo, lo destruye también (redundante con la llamada
  que ya existe en loseLife(), pero se deja aquí también como red de seguridad por si el enemigo
  se destruyera alguna vez por otra vía distinta a loseLife(), por ejemplo en una futura función
  de "limpiar nivel").
Cualquier objeto que cree sus propios timers/eventos/objetos hijos debe limpiarlos en su propio
destroy(); Phaser no lo adivina por sí solo.

*/
