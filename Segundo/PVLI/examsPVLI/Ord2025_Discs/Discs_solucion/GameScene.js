import Player from "./Player.js";
import Enemy from "./Enemy.js";

export default class GameScene extends Phaser.Scene {
	constructor() {
		super({ key: 'GameScene' });
	}

	preload() {
		// aquí cargamos sprites, sonidos, etc.
		this.load.image('bg', 'assets/bg.png');
		this.load.image('longBarrier', 'assets/long_barrier.png');
		this.load.image('shortBarrier', 'assets/short_barrier.png');
		this.load.image('disc', 'assets/disc.png');
		this.load.image('discOff', 'assets/disc_off.png');
		// Esto ya son spritesheets, no solo sprites únicos
		this.load.spritesheet('player', 'assets/player_sheet.png',
			{ frameWidth: 32, frameHeight: 32 });
		this.load.spritesheet('enemy', 'assets/enemy_sheet.png',
			{ frameWidth: 32, frameHeight: 32 });

		// Audio
		this.load.audio('playerHit', 'assets/sounds/player_hit.mp3');
		this.load.audio('enemyDeath', 'assets/sounds/enemy_death.mp3');
		this.load.audio('loose', 'assets/sounds/loose.mp3');
	}

	create() {
		// Variables auxiliares guapas guapas
		// Centro del CANVAS
		let centerX = this.sys.game.config.width / 2;
		let centerY = this.sys.game.config.height / 2;

		// Fondo
		this.add.image(0, 0, 'bg').setOrigin(0, 0);

		// Barreras como cuerpos físicos estáticos
		// (inmóviles, no las afecta la gravedad/física)
		this.barriers = this.physics.add.staticGroup();
		this.barriers.create(250, 59, 'longBarrier');   // arriba
		this.barriers.create(250, 278, 'longBarrier');  // abajo
		this.barriers.create(23, 168, 'shortBarrier');  // izquierda
		this.barriers.create(477, 168, 'shortBarrier'); // derecha

		// Límites de la zona jugable (coinciden con las barreras)
		this.fieldBounds = {
			minX: 36,
			maxX: 464,
			minY: 72,
			maxY: 265
		};

		// Anims
		// Guarda: this.anims es el AnimationManager GLOBAL del juego (no de la escena),
		// así que si jugamos una partida, perdemos, volvemos al menú y entramos otra vez
		// a GameScene, create() se vuelve a ejecutar entero y, sin esta comprobación,
		// intentaría crear las mismas claves de animación dos veces (Phaser lo permite
		// pero lanza warnings en consola). Con el if, solo se crean la primera vez.
		if (!this.anims.exists('player_idle')) {
			this.anims.create({
				key: 'player_idle',
				frames: this.anims.generateFrameNumbers('player', { start: 2, end: 2 }),
				frameRate: 5,
				repeat: -1
			});

			this.anims.create({
				key: 'player_run',
				frames: this.anims.generateFrameNumbers('player', { start: 0, end: 3 }),
				frameRate: 5,
				repeat: -1
			});

			this.anims.create({
				key: 'enemy_run',
				frames: this.anims.generateFrameNumbers('enemy', { start: 0, end: 2 }),
				frameRate: 5,
				repeat: -1
			});
		}

		// Player
		this.player = new Player(this, centerX, centerY);
		this.physics.add.collider(this.player, this.barriers);

		// Grupo de enemigos
		this.enemies = this.physics.add.group();
		this.physics.add.collider(this.enemies, this.barriers, this.onEnemyHitBarrier, null, this);

		// Oleadas
		this.waveNumber = 0;
		this.nextWaveScheduled = false;
		// Primera oleada a los 2 segundos de empezar
		this.time.delayedCall(2000, () => this.spawnWave());

		// Marcador
		this.score = 0;
		this.scoreText = this.add.text(this.sys.game.config.width / 2, 10, '00000', {
			fontFamily: 'arcade',
			fontSize: '32px',
			color: '#ffffff'
		}).setOrigin(0.5, 0);
	}

	update() {
		this.player.update();

		this.enemies.getChildren().forEach(enemy => enemy.update());

		// Cuando solo queda 1 enemigo vivo, programamos la siguiente oleada
		if (this.enemies.countActive() === 1 && !this.nextWaveScheduled) {
			this.nextWaveScheduled = true;
			this.time.delayedCall(3000, () => {
				this.spawnWave();
				this.nextWaveScheduled = false;
			});
		}
	}

	onEnemyHitBarrier(enemy, barrier) {
		enemy.reverseDirection();
	}

	spawnWave() {
		this.waveNumber++;

		// Cada 3ª oleada, uno de los 3 enemigos es verde (2 vidas)
		let isSpecialWave = this.waveNumber % 3 === 0;
		let greenIndex = isSpecialWave ? Phaser.Math.Between(0, 2) : -1;

		// Velocidad sube con la oleada, con tope razonable
		let speed = Math.min(30 + this.waveNumber * 10, 200);

		for (let i = 0; i < 3; i++) {
			let lives = (i === greenIndex) ? 2 : 1;
			let x = Phaser.Math.Between(this.fieldBounds.minX + 20, this.fieldBounds.maxX - 20);
			let y = Phaser.Math.Between(this.fieldBounds.minY + 20, this.fieldBounds.maxY - 20);

			let enemy = new Enemy(this, x, y, speed, lives);
			this.enemies.add(enemy);
			// ⚠️ BUG TRAMPA: physics.add.group() resetea body (velocidad, gravedad...) al hacer add().
			// Cualquier setVelocity / setAllowGravity que hayas puesto ANTES de add() se pierde.
			// SOLUCIÓN: llamar siempre a setRandomDirection() (o lo que toque) DESPUÉS del add().
            enemy.setRandomDirection();
		}
	}

	addScore(points) {
		this.score += points;
		this.scoreText.setText(String(this.score).padStart(5, '0'));
	}
}

/* EXPLICACIONES

ESTRUCTURA GENERAL
-------------------
GameScene es el corazón del proyecto: la escena donde transcurre la partida real. A diferencia
de MenuScene y GameOverScene, sí tiene preload() (carga todos los assets jugables) y un update()
activo (orquesta el movimiento del jugador, de cada enemigo, y la lógica de oleadas). Es la
única escena que "fabrica" otros objetos (Player, Enemy) en vez de limitarse a mostrar texto.

preload()
-----------
Carga, por este orden: imagen de fondo, las dos barreras, las dos texturas del disco (lanzado y
"apagado"), los DOS spritesheets de personajes (player y enemy, ambos con frames de 32x32 fijos,
visibles en sizes.txt del material del examen aunque aquí no se haya usado ese archivo
directamente) y los tres audios (golpe al jugador, muerte de enemigo, derrota). load.image() se
usa para texturas de un solo frame; load.spritesheet() para las que tienen varios frames
trocados en una rejilla regular, necesario para poder definir animaciones sobre ellas después
con generateFrameNumbers(). load.audio() simplemente decodifica el mp3 y lo deja listo para
reproducirse con this.sound.play()/this.sound.add() más adelante. Todo el preload() es
puramente declarativo: Phaser se encarga de descargar todo en paralelo y de esperar a que
termine antes de pasar a create() automáticamente, sin que tengamos que gestionar promesas ni
callbacks de carga a mano.

create() — orden y razones
------------------------------
1. centerX/centerY: calculados desde this.sys.game.config (el objeto de configuración completo
   que se definió en game.js), para no hardcodear el número 250/150 directamente y que el código
   se adapte solo si algún día cambiara el tamaño del canvas.

2. Fondo: una imagen simple sin física, dibujada en (0,0) con origin (0,0) (esquina superior
   izquierda, no centro) para que cubra exactamente todo el canvas desde la esquina.

3. Barreras: un staticGroup (cuerpos físicos que NUNCA se mueven, ideal para paredes — más
   eficiente que un grupo dinámico porque Arcade Physics no necesita recalcular su posición cada
   frame). Las 4 posiciones (250,59 / 250,278 / 23,168 / 477,168) se calcularon analizando
   píxel a píxel la imagen de referencia field.png para que coincidan exactamente con el marco
   morado dibujado en el arte original.

4. fieldBounds: un objeto propio (no de Phaser) con los límites de la zona realmente jugable
   (donde puede haber personajes), calculados igual que las barreras. Se usa al elegir
   posiciones de spawn aleatorias para los enemigos en spawnWave(), asegurando que no aparezcan
   dentro de una barrera ni fuera del campo.

5. Animaciones (con guarda): this.anims es el ANIMATION MANAGER GLOBAL del juego completo (vive
   a nivel de Phaser.Game, no se destruye al cambiar de escena, a diferencia de this.add,
   this.physics, etc. que sí son por-escena). Esto significa que si el jugador pierde, vuelve al
   menú, y entra otra vez a GameScene, este create() se ejecuta de cero otra vez — y sin la
   comprobación if (!this.anims.exists('player_idle')), intentaría volver a registrar las
   mismas claves de animación ('player_idle', 'player_run', 'enemy_run'), lo cual Phaser permite
   técnicamente pero generando warnings en consola en cada repetición. Con la guarda, solo se
   crean realmente la primera vez que se entra a la escena en toda la sesión del navegador.

6. Player: se instancia en el centro del canvas y se le añade un collider contra las barreras
   (para que no las atraviese al moverse).

7. Grupo de enemigos: this.physics.add.group() crea un grupo DINÁMICO (a diferencia del
   staticGroup de las barreras). Se le añade un único collider contra this.barriers, válido para
   TODOS los enemigos presentes y futuros que se añadan al grupo — no hace falta registrar un
   collider individual por cada enemigo nuevo, Phaser comprueba automáticamente cada miembro del
   grupo contra las barreras en cada frame.

8. Oleadas: waveNumber empieza en 0 (se incrementa dentro de spawnWave() antes de usarse, así la
   primera oleada real es la 1). nextWaveScheduled es una bandera de cooldown para evitar
   programar la siguiente oleada más de una vez mientras la condición "queda 1 enemigo" se siga
   cumpliendo en varios frames seguidos. this.time.delayedCall(2000, ...) dispara la primera
   oleada exactamente a los 2 segundos de empezar la partida, tal y como pide el enunciado.

9. Marcador: Text centrado horizontalmente (setOrigin(0.5, 0) centra en X pero mantiene el
   anclaje arriba en Y), inicializado a '00000' como placeholder con formato de 5 dígitos de
   estilo arcade clásico.

update()
----------
Se ejecuta automáticamente cada frame por Phaser (es el único método de los 4 "especiales" de
una Scene — init, preload, create, update — que se repite continuamente en vez de ejecutarse
una sola vez).

1. this.player.update(): delega toda la lógica de movimiento/input/disco del jugador a su
   propia clase. GameScene no necesita saber CÓMO se mueve el jugador, solo que tiene que
   "darle paso" cada frame.

2. this.enemies.getChildren().forEach(enemy => enemy.update()): igual que con el jugador, pero
   iterando todos los enemigos vivos del grupo. getChildren() devuelve un array con los
   miembros activos (los destruidos se autoeliminan del grupo, así que este array siempre está
   actualizado sin que tengamos que filtrarlo a mano).

3. Comprobación de oleada: this.enemies.countActive() cuenta cuántos enemigos del grupo siguen
   activos ahora mismo. countActive(true) (con true explícito) cuenta solo los que tienen
   active=true; sin argumento, true es el valor por defecto, así que countActive() a secas
   también sirve. Cuando llega a exactamente 1 (queda un único enemigo en pantalla, tal y como
   pide el enunciado para la siguiente oleada) Y no hay ya una oleada programada
   (!nextWaveScheduled), se activa la bandera y se programa spawnWave() a los 3 segundos.

onEnemyHitBarrier(enemy, barrier)
-------------------------------------
Callback registrado en el collider del grupo de enemigos. Phaser lo llama automáticamente cada
vez que CUALQUIER enemigo del grupo choca contra CUALQUIER barrera, pasándole como argumentos
los dos objetos concretos implicados en esa colisión particular. Aquí solo nos interesa el
primero (enemy): delegamos en su propio método reverseDirection() toda la lógica de qué hacer
al rebotar (ver Enemy.js).

spawnWave()
-------------
1. Incrementa el contador de oleada.
2. isSpecialWave: true si el número de oleada es múltiplo de 3 (waveNumber % 3 === 0 — el
   operador % da el resto de la división, y un resto de 0 significa que es divisible
   exactamente). greenIndex elige al azar, SOLO si es oleada especial, cuál de las 3 posiciones
   del bucle (0, 1 o 2) será el enemigo de 2 vidas; si no es oleada especial, greenIndex vale -1,
   un valor que nunca puede coincidir con ningún índice real del bucle (0,1,2), garantizando que
   ningún enemigo sea verde esa oleada.
3. speed: fórmula 30 + waveNumber*10, con un tope duro de 200 vía Math.min() — así la dificultad
   sube progresivamente pero nunca se vuelve literalmente injugable a partir de cierta oleada.
4. Bucle de 3 iteraciones (los 3 enemigos por oleada que pide el enunciado): calcula vidas (2
   solo si i === greenIndex), una posición aleatoria dentro de fieldBounds con un margen de 20px
   para no aparecer pegados a una barrera, crea el Enemy, lo añade al grupo, y — crucialmente —
   vuelve a llamar a enemy.setRandomDirection() DESPUÉS de añadirlo al grupo (ver la explicación
   detallada de por qué en Enemy.js; en resumen, el grupo resetea la velocidad al añadir un
   miembro, así que hay que reaplicarla después, no antes).

addScore(points)
-------------------
Suma puntos al contador interno y actualiza el texto visible. String(this.score) convierte
explícitamente el número a texto antes de poder usar .padStart(5, '0') (un método de String
que rellena por la izquierda con ceros hasta alcanzar 5 caracteres de longitud) — sin esta
conversión explícita, padStart no existiría sobre un valor numérico. El resultado es el típico
marcador arcade de 5 dígitos con ceros a la izquierda (p.ej. "00150" en vez de simplemente
"150"), coherente con el texto inicial '00000' que se puso en create().

*/
