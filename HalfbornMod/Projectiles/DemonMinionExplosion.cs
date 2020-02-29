using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace HalfbornMod.Projectiles
{

    public class DemonMinionExplosion : ModProjectile
    {

        public override void SetDefaults()
        {
            base.projectile.width = 98;
            base.projectile.height = 98;
            base.projectile.aiStyle = -1;
            base.projectile.friendly = true;
            base.projectile.penetrate = -1;
            base.projectile.timeLeft = 15;
            base.projectile.tileCollide = false;
            Main.projFrames[base.projectile.type] = 7;
        }


        public override void AI()
        {
            int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y) - base.projectile.velocity * 0.5f, base.projectile.width - 8, base.projectile.height - 8, 229, 0f, 0f, 100, default(Color), 1.15f);
            Dust dust = Main.dust[num];
            dust = Main.dust[num];
            dust.velocity *= 0.2f;
            Main.dust[num].noGravity = true;
        }


        public override void PostAI()
        {
            base.projectile.frameCounter++;
            if (base.projectile.frameCounter > 2)
            {
                base.projectile.frame++;
                base.projectile.frameCounter = 0;
            }
            if (base.projectile.frame >= 7)
            {
                base.projectile.frame = 0;
            }
        }
    }
}
