export default class Enemy extends Phaser.Physics.Arcade.Sprite {
    constructor(scene, x, y, platformLeft, platformRight) {
        super(scene, x, y, 'enemy');

        scene.add.existing(this);
        scene.physics.add.existing(this);

        this.setOrigin(0.5);

        this.left = platformLeft;
        this.right = platformRight;

        this.speed = 30;
        // dirección aleatoria al aparecer
        this.facing = Phaser.Math.Between(0, 1) === 0 ? 'left' : 'right';

        this.stunned = false;
        this.canShoot = true;

        this.play('enemy_run');

        this.scheduleNextThrow();
    }

    update() {
        if (this.stunned) return;

        // rebota en los límites de SU plataforma
        if (this.x <= this.left) {
            this.facing = 'right';
            this.setVelocityX(this.speed);
        } else if (this.x >= this.right) {
            this.facing = 'left';
            this.setVelocityX(-this.speed);
        }

        this.setFlipX(this.facing === 'left');
    }

    scheduleNextThrow() {
        let delay = Phaser.Math.Between(1000, 2500);
        this.throwTimer = this.scene.time.delayedCall(delay, () => {
            if (!this.active) return; // enemigo ya destruido, no reprogramar nada más
            if (!this.stunned) {
                this.shoot();
            }
            this.scheduleNextThrow();
        });
    }

    shoot() {
        this.scene.spawnShuriken(this.x, this.y, this.facing, 'enemy');
    }

    getStunned() {
        if (!this.active) return;
        this.stunned = true;
        this.setVelocityX(0);
        this.play('enemy_stun');

        this.scene.time.delayedCall(2000, () => {
            this.stunned = false;
            this.play('enemy_run');
        });
    }

    die() {
        if (this.throwTimer) this.throwTimer.remove();
        if (this.stunTimer) this.stunTimer.remove();
        this.setVelocityX(0);
        this.play('enemy_die');
        this.scene.sound.play('enemy_die');
        this.scene.time.delayedCall(500, () => {
            this.destroy();
        });
    }
}