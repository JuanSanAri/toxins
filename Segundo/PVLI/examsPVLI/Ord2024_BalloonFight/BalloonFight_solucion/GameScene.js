import Player from "./Player.js";
import Enemy from "./Enemy.js";

export default class GameScene extends Phaser.Scene {
	constructor() {
		super({ key: 'gameScene' });
	}

	preload() {
		// tilemap exportado desde Tiled (JSON) + imagen del tileset
		this.load.tilemapTiledJSON('terrain', 'assets/terrain.json');
		this.load.image('tileset', 'assets/tileset.png');

		// spritesheets del jugador: doll, globo y muerte (texturas separadas)
		this.load.spritesheet('player', 'assets/player.png', { frameWidth: 14, frameHeight: 14 });
		this.load.spritesheet('balloon', 'assets/balloon.png', { frameWidth: 12, frameHeight: 12 });
		this.load.spritesheet('dead', 'assets/dead.png', { frameWidth: 16, frameHeight: 16 });

		this.load.spritesheet('enemy', 'assets/enemy.png', { frameWidth: 16, frameHeight: 16 });

		this.load.image('cloud', 'assets/cloud.png'); // imagen simple, sin física
		this.load.spritesheet('star', 'assets/star.png', { frameWidth: 10, frameHeight: 10 });
		this.load.spritesheet('splash', 'assets/splash.png', { frameWidth: 16, frameHeight: 16 });

		this.load.audio('balloon', 'assets/balloon.mp3');
		this.load.audio('loose', 'assets/loose.mp3');
		this.load.audio('victory', 'assets/victory.mp3');
	}

	create() {
        if (!this.anims.exists('player_idle')) {
            this.anims.create({
                key: 'player_idle',
                frames: this.anims.generateFrameNumbers('player', { start: 6, end: 6 }),
                frameRate: 5, repeat: -1
            });
            this.anims.create({
                key: 'player_fly',
                frames: this.anims.generateFrameNumbers('player', { start: 6, end: 8 }),
                frameRate: 8, repeat: -1
            });
            this.anims.create({
                key: 'player_dead',
                frames: this.anims.generateFrameNumbers('dead', { start: 0, end: 2 }),
                frameRate: 8, repeat: 0 // no loop, se reproduce una sola vez
            });
            this.anims.create({
                key: 'enemy_fly',
                frames: this.anims.generateFrameNumbers('enemy', { start: 3, end: 5 }),
                frameRate: 8, repeat: -1
            });
            this.anims.create({
                key: 'enemy_fall',
                frames: this.anims.generateFrameNumbers('enemy', { start: 0, end: 2 }),
                frameRate: 8, repeat: -1
            });
            this.anims.create({
                key: 'enemy_dead',
                frames: this.anims.generateFrameNumbers('enemy', { start: 6, end: 6 }),
                frameRate: 5, repeat: 0
            });
            this.anims.create({
                key: 'balloon_normal',
                frames: this.anims.generateFrameNumbers('balloon', { start: 0, end: 0 }),
                frameRate: 5, repeat: -1
            });
            this.anims.create({
                key: 'balloon_pop',
                frames: this.anims.generateFrameNumbers('balloon', { start: 1, end: 1 }),
                frameRate: 5, repeat: 0
            });
            this.anims.create({
                key: 'star_fly',
                frames: this.anims.generateFrameNumbers('star', { start: 0, end: 4 }),
                frameRate: 12,
                repeat: -1
            });
            this.anims.create({
                key: 'splash_anim',
                frames: this.anims.generateFrameNumbers('splash', { start: 0, end: 3 }),
                frameRate: 10,
                repeat: 0
            });
        }

        let centerX = this.sys.game.config.width / 2;

        const map = this.make.tilemap({ key: 'terrain' });
        const tileset = map.addTilesetImage('tileset', 'tileset');

        this.waterLayer = map.createLayer('water', tileset, 0, 0);
        this.landLayer = map.createLayer('land', tileset, 0, 0);

        /*
        // Colisiones tiles (COMENTAR BLOQUE CUANDO SE ACABE)

        const debugGraphics = this.add.graphics().setAlpha(0.7);
        this.landLayer.renderDebug(debugGraphics, {
            tileColor: null, // tiles sin colisión: no se dibujan
            // tiles con colisión: naranja
            collidingTileColor: new Phaser.Display.Color(243, 134, 48, 200),
            // bordes de colisión: morado
            faceColor: new Phaser.Display.Color(200, 0, 237, 255)
        });*/

		// setCollisionByExclusion([-1]): hace colisionable toda tile que NO sea vacía (-1 = sin tile)
		this.waterLayer.setCollisionByExclusion([-1]);
		this.landLayer.setCollisionByExclusion([-1]);

		this.player = new Player(this, centerX, 160);
		this.physics.add.collider(this.player.doll, this.landLayer); // el muñeco colisiona con las plataformas

		// collider con el agua: rebota y genera splash (no "bucea")
		this.canSplash = true; // flag de cooldown para no spawnear splash 60 veces por segundo mientras toca el agua
		this.physics.add.collider(this.player.doll, this.waterLayer, (playerDoll, tile) => {
			if (!this.canSplash) return;

			this.canSplash = false;
			this.spawnSplash(playerDoll.x, tile.pixelY); // tile.pixelY = borde superior del tile tocado, en píxeles
			this.time.delayedCall(500, () => {
				this.canSplash = true;
			});
		});

		// posiciones de spawn dispersas por el mapa (ajustar si caen dentro de plataformas)
		const spawnPoints = [
			{ x: 60, y: 60 },
			{ x: 200, y: 60 },
			{ x: 350, y: 60 },
			{ x: 120, y: 200 }
		];

		this.enemies = []; // array propio (no grupo de Phaser) porque gestionamos los overlaps individualmente
		for (let sp of spawnPoints) {
			let enemy = new Enemy(this, sp.x, sp.y);
			this.physics.add.collider(enemy.doll, this.landLayer);

			// overlap 1: el muñeco del jugador toca el GLOBO del enemigo -> lo revienta
			this.physics.add.overlap(this.player.doll, enemy.balloon, () => {
				if (enemy.hasBalloon && this.player.hasBalloon) { // guarda doble: ambos deben tener globo
					enemy.popBalloon();
					this.addScore(300);
					this.sound.play('balloon');
				}
			});

			// collider con processCallback: solo colisiona físicamente si el enemigo NO tiene globo
			this.physics.add.collider(
				this.player.doll,
				enemy.doll,
				(playerDoll, enemyDoll) => {
					this.player.doll.setVelocityY(-30); // pequeño impulso de rebote hacia arriba al jugador

					// solo suma puntos y mata si: enemigo sin globo, no invulnerable, jugador vivo, enemigo en el suelo
					let enemyGrounded = Math.abs(enemy.doll.body.velocity.y) < 5; // umbral en vez de === 0 (coma flotante)
					if (!enemy.hasBalloon && !enemy.invulnerableAfterPop && this.player.hasBalloon && enemyGrounded) {
						this.addScore(500);
						enemy.die();
						this.enemies = this.enemies.filter(e => e !== enemy); // filter crea un array nuevo sin el enemigo muerto
						this.checkVictory();
					}
				},
				(playerDoll, enemyDoll) => !enemy.hasBalloon, // processCallback: false = Phaser ignora la colisión este frame
				this
			);

			// overlap 3: el MUÑECO del enemigo toca el GLOBO del jugador -> lo revienta
			this.physics.add.overlap(enemy.doll, this.player.balloon, () => {
				if (this.player.hasBalloon && enemy.hasBalloon) { // el enemigo solo puede dañar si él también tiene globo
					this.player.popBalloon();
					this.sound.play('balloon');
					this.onDefeat();
				}
			});

			this.enemies.push(enemy);
		}

		// nube aparece a los 35s
		this.time.delayedCall(35000, () => this.spawnCloud());

		this.score = 0;
		// setDepth(10) garantiza que el texto quede siempre encima de cualquier objeto creado (la nube, el rayo...)
		this.scoreText = this.add.text(centerX, 3, 'SCORE: 0', {
			fontFamily: 'arcade',
			fontSize: '16px',
			color: '#ffffff'
		}).setOrigin(0.5, 0).setDepth(10);
	}

	update() {
		this.player.update();
		this.enemies.forEach(e => e.update()); // Phaser no llama solo al update() de objetos custom, hay que delegarlo a mano
	}

	addScore(points) {
		this.score += points;
		this.scoreText.setText('Score: ' + this.score); // setText actualiza el texto sin destruir y recrear el objeto
	}

	checkVictory() {
		if (this.enemies.length === 0) { // array vacío = todos los enemigos eliminados
			this.sound.play('victory');
			this.time.delayedCall(2000, () => {
				this.scene.start('GameOver', { win: true, score: this.score });
			});
		}
	}

	onDefeat() {
		this.sound.play('loose');
		this.time.delayedCall(2000, () => {
			this.scene.start('GameOver', { win: false, score: this.score });
		});
	}

	spawnCloud() {
		let x = Phaser.Math.Between(40, this.sys.game.config.width - 40); // posición X aleatoria con margen en los bordes
		this.cloud = this.add.image(x, 20, 'cloud'); // imagen simple, sin física

		this.time.delayedCall(2000, () => this.spawnStar()); // 2s de "dramatic pause" antes de lanzar el rayo
	}

	spawnStar() {
		this.star = this.physics.add.sprite(this.cloud.x, this.cloud.y + 16, 'star'); // nace justo debajo de la nube
		this.star.play('star_fly');
		this.star.setBounce(1, 1); // rebote elástico perfecto (conserva toda la velocidad al chocar)
		this.star.setCollideWorldBounds(true); // rebota también en los bordes del canvas
		this.star.body.setAllowGravity(false); // flota, no cae

		let angle = Phaser.Math.Between(0, 359); // ángulo aleatorio para que no sea predecible
		let velocity = this.physics.velocityFromAngle(angle, 150); // helper que hace la trigonometría por nosotros
		this.star.setVelocity(velocity.x, velocity.y);

		this.physics.add.collider(this.star, this.landLayer); // rebota en las plataformas

		this.physics.add.overlap(this.star, this.player.doll, () => {
			if (this.player.hasBalloon) { // guarda: no hace nada si el jugador ya estaba muerto
				this.player.popBalloon();
				this.sound.play('loose');
				this.onDefeat();
			}
		});
	}

	spawnSplash(x, y) {
		let splash = this.add.sprite(x, y, 'splash'); // add.sprite sin física, solo visual
		splash.play('splash_anim');
		splash.once('animationcomplete', () => splash.destroy()); // se destruye solo al acabar la anim (repeat: 0)
	}
}