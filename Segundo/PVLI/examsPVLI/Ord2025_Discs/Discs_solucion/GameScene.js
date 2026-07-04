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
