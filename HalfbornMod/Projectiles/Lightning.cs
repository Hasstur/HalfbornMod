using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace HalfbornMod.Projectiles
{

    public class Lightning : ModProjectile
    {

        public override void SetDefaults()
        {
            base.projectile.width = 14;
            base.projectile.height = 14;
            base.projectile.aiStyle = -1;
            base.projectile.friendly = true;
            base.projectile.penetrate = -1;
            base.projectile.timeLeft = 600;
            base.projectile.extraUpdates = 7;
            ProjectileID.Sets.TrailingMode[base.projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 90;
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int i = 0; i < base.projectile.oldPos.Length; i++)
            {
                if (base.projectile.oldPos[i] == Vector2.Zero)
                {
                    return false;
                }
                Vector2 position = base.projectile.oldPos[i] - Main.screenPosition + base.projectile.Size / 2f;
                spriteBatch.Draw(base.mod.GetTexture("Projectiles/MJLight_Glow"), position, null, Color.White, 0f, base.projectile.Size / 2f, 0.65f, SpriteEffects.None, 0f);
            }
            return true;
        }


        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int i = 0; i < base.projectile.oldPos.Length; i++)
            {
                if (base.projectile.oldPos[i] == Vector2.Zero)
                {
                    return;
                }
                Vector2 position = base.projectile.oldPos[i] - Main.screenPosition + base.projectile.Size / 2f;
                spriteBatch.Draw(Main.projectileTexture[base.projectile.type], position, null, Color.White, 0f, base.projectile.Size / 2f, 0.5f, SpriteEffects.None, 0f);
            }
        }


        public override void OnHitNPC(NPC npc, int damage, float knockback, bool crit)
        {
            npc.immune[base.projectile.owner] = 5;
        }


        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 2;
        }


        public override void AI()
        {
            Player player = Main.player[base.projectile.owner];
            if (Vector2.Distance(player.Center, base.projectile.Center) > 1000f)
            {
                base.projectile.Kill();
            }
            if (base.projectile.velocity == Vector2.Zero)
            {
                float num = base.projectile.rotation + 1.57079637f + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float num2 = (float)Main.rand.NextDouble() * 1.25f + 1.25f;
                Vector2 vector = new Vector2((float)Math.Cos((double)num) * num2, (float)Math.Sin((double)num) * num2);
                int num3 = Dust.NewDust(base.projectile.oldPos[base.projectile.oldPos.Length - 1], 0, 0, 107, vector.X, vector.Y, 0, default(Color), 1f);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].scale = 1f;
            }
            if (base.projectile.timeLeft < 598)
            {
                if (base.projectile.localAI[1] == 0f && base.projectile.ai[0] >= 900f)
                {
                    base.projectile.ai[0] -= 1000f;
                    base.projectile.localAI[1] = -1f;
                }
                int num4 = base.projectile.frameCounter;
                base.projectile.frameCounter = num4 + 1;
                Lighting.AddLight(base.projectile.Center, 0.1f, 0.45f, 0.5f);
                if (base.projectile.velocity == Vector2.Zero)
                {
                    if (base.projectile.frameCounter >= base.projectile.extraUpdates * 2)
                    {
                        base.projectile.frameCounter = 0;
                        bool flag = true;
                        for (int i = 1; i < base.projectile.oldPos.Length; i = num4 + 1)
                        {
                            if (base.projectile.oldPos[i] != base.projectile.oldPos[0])
                            {
                                flag = false;
                            }
                            num4 = i;
                        }
                        if (flag)
                        {
                            base.projectile.Kill();
                        }
                    }
                    if (Main.rand.Next(base.projectile.extraUpdates) == 0 && (base.projectile.velocity != Vector2.Zero || Main.rand.Next((base.projectile.localAI[1] == 2f) ? 2 : 6) == 0))
                    {
                        for (int j = 0; j < 2; j = num4 + 1)
                        {
                            float num5 = base.projectile.rotation + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                            float num6 = (float)Main.rand.NextDouble() * 0.8f + 1f;
                            Vector2 vector2 = new Vector2((float)Math.Cos((double)num5) * num6, (float)Math.Sin((double)num5) * num6);
                            int num7 = Dust.NewDust(base.projectile.Center, 0, 0, 107, vector2.X, vector2.Y, 0, default(Color), 1f);
                            Main.dust[num7].noGravity = true;
                            Main.dust[num7].scale = 1.2f;
                            num4 = j;
                        }
                        if (Main.rand.Next(5) == 0)
                        {
                            Vector2 value = Utils.RotatedBy(base.projectile.velocity, 1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)base.projectile.width;
                            int num8 = Dust.NewDust(base.projectile.Center + value - Vector2.One * 4f, 8, 8, 107, 0f, 0f, 100, default(Color), 1.5f);
                            Dust dust = Main.dust[num8];
                            dust.velocity *= 0.5f;
                            Main.dust[num8].velocity.Y = -Math.Abs(Main.dust[num8].velocity.Y);
                            return;
                        }
                    }
                }
                else if (base.projectile.frameCounter >= base.projectile.extraUpdates * 2)
                {
                    base.projectile.frameCounter = 0;
                    float num9 = base.projectile.velocity.Length();
                    UnifiedRandom unifiedRandom = new UnifiedRandom((int)base.projectile.ai[1]);
                    int num10 = 0;
                    Vector2 vector3 = -Vector2.UnitY;
                    Vector2 vector4;
                    do
                    {
                        int num11 = unifiedRandom.Next();
                        base.projectile.ai[1] = (float)num11;
                        num11 %= 100;
                        float num12 = (float)num11 / 100f * 6.28318548f;
                        vector4 = Utils.ToRotationVector2(num12);
                        if (vector4.Y > 0f)
                        {
                            vector4.Y *= -1f;
                        }
                        bool flag2 = false;
                        if (vector4.Y > -0.02f)
                        {
                            flag2 = true;
                        }
                        if (vector4.X * (float)(base.projectile.extraUpdates + 1) * 2f * num9 + base.projectile.localAI[0] > 35f)
                        {
                            flag2 = true;
                        }
                        if (vector4.X * (float)(base.projectile.extraUpdates + 1) * 2f * num9 + base.projectile.localAI[0] < -35f)
                        {
                            flag2 = true;
                        }
                        if (!flag2)
                        {
                            goto IL_637;
                        }
                        num4 = num10;
                        num10 = num4 + 1;
                    }
                    while (num4 < 100);
                    base.projectile.velocity = Vector2.Zero;
                    if (base.projectile.localAI[1] < 1f)
                    {
                        base.projectile.localAI[1] += 2f;
                        goto IL_63B;
                    }
                    goto IL_63B;
                    IL_637:
                    vector3 = vector4;
                    IL_63B:
                    if (base.projectile.velocity != Vector2.Zero)
                    {
                        base.projectile.localAI[0] += vector3.X * (float)(base.projectile.extraUpdates + 1) * 2f * num9;
                        base.projectile.velocity = Utils.RotatedBy(vector3, (double)(base.projectile.ai[0] + 1.57079637f), default(Vector2)) * num9;
                        base.projectile.rotation = Utils.ToRotation(base.projectile.velocity) + 1.57079637f;
                        if (Main.rand.Next(5) == 0 && Main.netMode != 1 && base.projectile.localAI[1] == 0f)
                        {
                            float num13 = (float)Main.rand.Next(-1, 1) * 1.04719758f / 3f;
                            Vector2 vector5 = Utils.RotatedBy(Utils.ToRotationVector2(base.projectile.ai[0]), (double)num13, default(Vector2)) * base.projectile.velocity.Length();
                            int num14 = Projectile.NewProjectile(base.projectile.Center.X - vector5.X, base.projectile.Center.Y - vector5.Y, vector5.X, vector5.Y, base.projectile.type, base.projectile.damage, base.projectile.knockBack, base.projectile.owner, Utils.ToRotation(vector5) + 1000f, base.projectile.ai[1]);
                            Main.projectile[num14].timeLeft = 240;
                        }
                    }
                }
            }
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            base.projectile.velocity.X = 0f;
            base.projectile.velocity.Y = 0f;
            /*if (!this.shock)
            {
                Main.PlaySound(4, (int)base.projectile.position.X, (int)base.projectile.position.Y, 56, 1f, 0f);
                int num = Main.rand.Next(3);
                if (num == 0)
                {
                    Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y + 50f, 0f, 0f, base.ModContent.ProjectileType("MJBoom3"), (int)((float)base.projectile.damage * 1.75f), 8f, base.projectile.owner, 0f, 0f);
                }
                if (num == 1)
                {
                    Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, 0f, 0f, base.ModContent.ProjectileType("MJBoom2"), (int)((float)base.projectile.damage * 1.5f), 8f, base.projectile.owner, 0f, 0f);
                }
                if (num == 2)
                {
                    Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y - 50f, 0f, 0f, base.ModContent.ProjectileType("MJBoom1"), (int)((float)base.projectile.damage * 1.25f), 8f, base.projectile.owner, 0f, 0f);
                }
            }*/
            return false;
        }


        //public bool shock;
    }
}
