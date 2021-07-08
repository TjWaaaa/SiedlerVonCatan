using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using NUnit.Framework;
using Player;

namespace Tests
{
    public class PlayerTest
    {
        [Test]
        public void resourceTest()
        {
            ServerPlayer testPlayer = new ServerPlayer(0);

            // Can change any resourcetypes to any positive amount
            testPlayer.setResourceAmount(RESOURCETYPE.SHEEP, 5);
            Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.SHEEP) == 5);
            testPlayer.setResourceAmount(RESOURCETYPE.WHEAT, 2);
            Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.WHEAT) == 2);
            testPlayer.setResourceAmount(RESOURCETYPE.SHEEP, -2);
            Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.SHEEP) == 3);

            // Can't change to a negative value
            Assert.That(() => testPlayer.setResourceAmount(RESOURCETYPE.ORE, -5),
                Throws.TypeOf<Exception>());
            Assert.IsFalse(testPlayer.getResourceAmount(RESOURCETYPE.ORE) == -5);

            // RESOURCETYPE.NONE isn't a real type. It can't have a value
            Assert.That(() => testPlayer.setResourceAmount(RESOURCETYPE.NONE, 5),
                Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public void canTradeTest()
        {
            ServerPlayer testPlayer = new ServerPlayer(0);

            testPlayer.setResourceAmount(RESOURCETYPE.SHEEP, 5);
            testPlayer.setResourceAmount(RESOURCETYPE.WHEAT, 3);
            Assert.IsTrue(testPlayer.canTrade(RESOURCETYPE.SHEEP));
            Assert.IsFalse(testPlayer.canTrade(RESOURCETYPE.ORE));
            Assert.IsFalse(testPlayer.canTrade(RESOURCETYPE.WHEAT));
        }

        [Test]
        public void tradeTest()
        {
            ServerPlayer testPlayer = new ServerPlayer(0);

            testPlayer.setResourceAmount(RESOURCETYPE.SHEEP, 5);
            testPlayer.setResourceAmount(RESOURCETYPE.WHEAT, 3);

            testPlayer.trade(new[] {4, 0, 0, 0, 0}, new[] {0, 0, 0, 0, 1});
            Assert.IsFalse(testPlayer.getResourceAmount(RESOURCETYPE.SHEEP) == 5);
            Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.SHEEP) == 1);
            Assert.IsFalse(testPlayer.getResourceAmount(RESOURCETYPE.WHEAT) == 3);
            Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.WHEAT) == 4);

        }

        [Test]
        public void canBuyBuyableTest()
        {
            ServerPlayer testPlayer = new ServerPlayer(0);

            Assert.IsFalse(testPlayer.canBuyBuyable(BUYABLES.ROAD));
            Assert.IsFalse(testPlayer.canBuyBuyable(BUYABLES.VILLAGE));
            Assert.IsFalse(testPlayer.canBuyBuyable(BUYABLES.CITY));
            Assert.IsFalse(testPlayer.canBuyBuyable(BUYABLES.DEVELOPMENT_CARDS));

            testPlayer.setResourceAmount(RESOURCETYPE.SHEEP, 1);
            testPlayer.setResourceAmount(RESOURCETYPE.ORE, 3);
            testPlayer.setResourceAmount(RESOURCETYPE.BRICK, 1);
            testPlayer.setResourceAmount(RESOURCETYPE.WOOD, 1);
            testPlayer.setResourceAmount(RESOURCETYPE.WHEAT, 2);

            Assert.IsTrue(testPlayer.canBuyBuyable(BUYABLES.ROAD));
            Assert.IsTrue(testPlayer.canBuyBuyable(BUYABLES.VILLAGE));
            Assert.IsTrue(testPlayer.canBuyBuyable(BUYABLES.CITY));
            Assert.IsTrue(testPlayer.canBuyBuyable(BUYABLES.DEVELOPMENT_CARDS));
        }

        [Test]
        public void buyBuyableTest()
        {
            ServerPlayer testPlayer = new ServerPlayer(0);

            int brickAmount = 3;
            int woodAmount = 3;
            int sheepAmount = 7;
            int oreAmount = 5;
            int wheatAmount = 4;
            testPlayer.setResourceAmount(RESOURCETYPE.BRICK, brickAmount);
            testPlayer.setResourceAmount(RESOURCETYPE.WOOD, woodAmount);
            testPlayer.setResourceAmount(RESOURCETYPE.SHEEP, sheepAmount);
            testPlayer.setResourceAmount(RESOURCETYPE.ORE, oreAmount);
            testPlayer.setResourceAmount(RESOURCETYPE.WHEAT, wheatAmount);

            for (int i = 0; i < 10; i++)
            {
                if (testPlayer.canBuyBuyable(BUYABLES.CITY))
                {
                    testPlayer.buyBuyable(BUYABLES.CITY);
                    wheatAmount -= 2;
                    oreAmount -= 3;

                }

                if (testPlayer.canBuyBuyable(BUYABLES.ROAD))
                {
                    testPlayer.buyBuyable(BUYABLES.ROAD);
                    brickAmount -= 1;
                    woodAmount -= 1;
                }

                if (testPlayer.canBuyBuyable(BUYABLES.VILLAGE))
                {
                    testPlayer.buyBuyable(BUYABLES.VILLAGE);
                    brickAmount -= 1;
                    woodAmount -= 1;
                    wheatAmount -= 1;
                    sheepAmount -= 1;
                }

                if (testPlayer.canBuyBuyable(BUYABLES.DEVELOPMENT_CARDS))
                {
                    testPlayer.buyBuyable(BUYABLES.DEVELOPMENT_CARDS);
                    wheatAmount -= 1;
                    sheepAmount -= 1;
                    oreAmount -= 1;
                }

                Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.BRICK) == brickAmount);
                Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.WOOD) == woodAmount);
                Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.SHEEP) == sheepAmount);
                Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.WHEAT) == wheatAmount);
                Assert.IsTrue(testPlayer.getResourceAmount(RESOURCETYPE.ORE) == oreAmount);

            }


        }
    }
}