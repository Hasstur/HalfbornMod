
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HalfbornMod.Projectiles
{
    public class Bubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 3;
            projectile.ignoreWater = true; 
            projectile.tileCollide = false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Inflate some target hitboxes if they are beyond 8,8 size
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            // Return if the hitboxes intersects, which means the javelin collides or not
            return projHitbox.Intersects(targetHitbox);
        }


        // 
        /*
		 * The following showcases recommended practice to work with the ai field
		 * You make a property that uses the ai as backing field
		 * This allows you to contextualize ai better in the code
		 */

        // Are we sticking to a target?
        public bool IsStickingToTarget
        {
            get => projectile.ai[0] == 1f;
            set => projectile.ai[0] = value ? 1f : 0f;
        }

        // Index of the current target
        public int TargetWhoAmI
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        private const int MAX_STICKY_JAVELINS = 6; // This is the max. amount of javelins being able to attach
        private readonly Point[] _stickingJavelins = new Point[MAX_STICKY_JAVELINS]; // The point array holding for sticking javelins

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            IsStickingToTarget = true; // we are sticking to a target
            TargetWhoAmI = target.whoAmI; // Set the target whoAmI
            target.velocity =
                (target.Center - projectile.Center) *
                0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            projectile.netUpdate = true; // netUpdate this javelin

            projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore

            // It is recommended to split your code into separate methods to keep code clean and clear
           // UpdateStickyJavelins(target);
        }
        private const int MAX_TICKS = 45;

        public override void AI()
        {
            // Run either the Sticky AI or Normal AI
            // Separating into different methods helps keeps your AI clean
            if (IsStickingToTarget) StickyAI();
            else NormalAI();
        }

        private void NormalAI()
        {
            TargetWhoAmI++;

            // For a little while, the javelin will travel with the same speed, but after this, the javelin drops velocity very quickly.
            if (TargetWhoAmI >= MAX_TICKS)
            {               
                TargetWhoAmI = MAX_TICKS; // set ai1 to maxTicks continuously
            }
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];

                {

                    float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                    float shootToY = target.position.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));


                    if (distance < 480f && !target.friendly && target.active)
                    {

                        distance = 3f / distance;


                        shootToX *= distance * 5;
                        shootToY *= distance * 5;


                        projectile.velocity.X = shootToX;
                        projectile.velocity.Y = shootToY;
                    }
                }
            }
        }

        private void StickyAI()
        {
            int projTargetIndex = (int)TargetWhoAmI;
            projectile.velocity.Y -= 0.5f;
            Main.npc[projTargetIndex].position = projectile.position;
            const int aiFactor = 5; // Change this factor to change the 'lifetime' of this sticking javelin
            projectile.localAI[0] += 1f;

            // Every 30 ticks, the javelin will perform a hit effect

            if (projectile.localAI[0] >= 60 * aiFactor || projTargetIndex < 0 || projTargetIndex >= 200)
            { // If the index is past its limits, kill it
                projectile.Kill();
            }
            else if (Main.npc[projTargetIndex].active)
            { // If the target is active and can take damage
              // Set the projectile's position relative to the target's center
                //projectile.Center = Main.npc[projTargetIndex].Center - projectile.velocity * 2f;
                //projectile.gfxOffY = Main.npc[projTargetIndex].gfxOffY;                
            }
            else
            { // Otherwise, kill the projectile
                projectile.Kill();
            }
        }
    }
}