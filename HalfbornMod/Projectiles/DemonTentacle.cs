using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace HalfbornMod.Projectiles
{

    public class DemonTentacle : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Tentacle");
        }


        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.MaxUpdates = 3;
            projectile.magic = true;
        }


        public override void AI()
        {
            if (base.projectile.velocity.X != base.projectile.velocity.X)
            {
                if (Math.Abs(base.projectile.velocity.X) < 1f)
                {
                    base.projectile.velocity.X = -base.projectile.velocity.X;
                }
                else
                {
                    base.projectile.Kill();
                }
            }
            if (base.projectile.velocity.Y != base.projectile.velocity.Y)
            {
                if (Math.Abs(base.projectile.velocity.Y) < 1f)
                {
                    base.projectile.velocity.Y = -base.projectile.velocity.Y;
                }
                else
                {
                    base.projectile.Kill();
                }
            }
            Vector2 center = base.projectile.Center;
            base.projectile.scale = 1f - base.projectile.localAI[0];
            base.projectile.width = (int)(20f * base.projectile.scale);
            base.projectile.height = base.projectile.width;
            base.projectile.position.X = center.X - (float)(base.projectile.width / 2);
            base.projectile.position.Y = center.Y - (float)(base.projectile.height / 2);
            if ((double)base.projectile.localAI[0] < 0.1)
            {
                base.projectile.localAI[0] += 0.01f;
            }
            else
            {
                base.projectile.localAI[0] += 0.025f;
            }
            if (base.projectile.localAI[0] >= 0.95f)
            {
                base.projectile.Kill();
            }
            base.projectile.velocity.X = base.projectile.velocity.X + base.projectile.ai[0] * 1.5f;
            base.projectile.velocity.Y = base.projectile.velocity.Y + base.projectile.ai[1] * 1.5f;
            if (base.projectile.velocity.Length() > 16f)
            {
                base.projectile.velocity.Normalize();
                base.projectile.velocity *= 16f;
            }
            base.projectile.ai[0] *= 1.05f;
            base.projectile.ai[1] *= 1.05f;
            if (base.projectile.scale < 1f)
            {
                int num = 0;
                while ((float)num < base.projectile.scale * 10f)
                {
                    int num2 = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 143, base.projectile.velocity.X, base.projectile.velocity.Y, 100, default(Color), 1.5f);
                    Main.dust[num2].position = (Main.dust[num2].position + base.projectile.Center) / 2f;
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].velocity *= 0.1f;
                    Main.dust[num2].velocity -= base.projectile.velocity * (1.3f - base.projectile.scale);
                    Main.dust[num2].fadeIn = (float)(100 + base.projectile.owner);
                    Main.dust[num2].scale += base.projectile.scale * 0.75f;
                    num++;
                }
            }
        }
    }
}
