export default class Player extends Phaser.Physics.Arcade.Sprite {
    constructor(scene, x, y) {
        super(scene, x, y, 'player');

        scene.add.existing(this);
        scene.physics.add.existing(this);

        this.setOrigin(0.5);
        this.setCollideWorldBounds(true);

        this.speed = 60;
        this.jumpSpeed = -150;
        this.facing = 'right';
        this.canAct = true; // solo puede saltar/disparar con una dirección pulsada

        this.keys = scene.input.keyboard.addKeys('LEFT,RIGHT,X,Z');

        this.canShoot = true;
        this.play('player_idle');
    }

    update() {
        let onGround = this.body.blocked.down || this.body.touching.down;
        let pressingLeft = this.keys.LEFT.isDown;
        let pressingRight = this.keys.RIGHT.isDown;
        let pressingDirection = pressingLeft || pressingRight;

        // en el aire no puede cambiar de dirección, solo mantiene la que tenía
        if (onGround) {
            if (pressingLeft) {
                this.setVelocityX(-this.speed);
                this.facing = 'left';
            } else if (pressingRight) {
                this.setVelocityX(this.speed);
                this.facing = 'right';
            } else {
                this.setVelocityX(0);
            }
        }

        this.setFlipX(this.facing === 'left');

        // animación
        if (!onGround) {
            this.play('player_jump', true);
        } else if (pressingDirection) {
            this.play('player_run', true);
        } else {
            this.play('player_idle', true);
        }

        // saltar: solo si está pulsando una dirección Y en el suelo
        if (Phaser.Input.Keyboard.JustDown(this.keys.X) && pressingDirection && onGround) {
            this.setVelocityY(this.jumpSpeed);
            this.scene.sound.play('jump');
        }

        // disparar: solo si está pulsando una dirección
        if (Phaser.Input.Keyboard.JustDown(this.keys.Z) && pressingDirection && this.canShoot) {
            this.shoot();
        }
    }

    shoot() {
        if (!this.canShoot) return;

        this.canShoot = false;
        this.play('player_shoot');
        this.scene.sound.play('shoot');
        this.scene.spawnShuriken(this.x, this.y, this.facing, 'player');

        this.scene.time.delayedCall(200, () => {
            this.canShoot = true;
        });
    }
}