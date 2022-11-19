using NUnit.Framework;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Tests
{
    /// <summary>
    /// Tests the <see cref="SpriteUtility"/> class.
    /// </summary>
    public class Test_SpriteUtility
    {
        /// <summary>
        /// The unity sprite.
        /// </summary>
        private Sprite _unitySprite;

        /// <summary>
        /// The custom sprite.
        /// </summary>
        private Sprite _customSprite;

        /// <summary>
        /// The custom sprite that has sprite mode multiple enabled.
        /// </summary>
        private Sprite _customSpriteMultiple;

        /// <summary>
        /// Te custom sprite that is part of an atlas.
        /// </summary>
        private Sprite _customSpriteAtlassed;

        /// <summary>
        /// Initializes the sprites.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            string artfolder = Path.Combine("Packages", "dtt.editorutilities", "Tests", "Editor", "Art");
            _unitySprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            _customSprite = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(artfolder, "CustomSquare.png"));
            _customSpriteMultiple = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(artfolder, "CustomSquare_Multiple.png"));
            _customSpriteAtlassed = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(artfolder, "CustomSquare_Atlas.png"));
        }

        /// <summary>
        /// Tests whether the utility can return the importer of a sprite.
        /// It expects a <see cref="ArgumentNullException"/> to be thrown if the sprite is null.
        /// </summary>
        [Test]
        public void Test_GetImporterOfSprite_Null()
        {
            // Arrange.
            TestDelegate action = () =>
           {
               // Act.
               SpriteUtility.GetImporterOfSprite(null);
           };

            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }

        /// <summary>
        /// Tests whether the utility can return the importer of a sprite.
        /// It expects the value to be null if the sprite is a Unity build in sprite.
        /// </summary>
        [Test]
        public void Test_GetImporterOfSprite_UnitySprite()
        {
            // Act.
            TextureImporter actual = SpriteUtility.GetImporterOfSprite(_unitySprite);

            // Assert.
            Assert.IsNull(actual, "Expected the returned texture importer to be null because of the unity sprite but it wasn't.");
        }

        /// <summary>
        /// Tests whether the utility can return the importer of a sprite.
        /// It expects a non-null value to be returned if the value is a custom sprite.
        /// </summary>
        [Test]
        public void Test_GetImporterOfSprite_CustomSprite()
        {
            // Act.
            TextureImporter actual = SpriteUtility.GetImporterOfSprite(_customSprite);

            // Assert.
            Assert.NotNull(actual);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is part of unity or not.
        /// It expects a <see cref="ArgumentNullException"/> to be thrown if the sprite is null.
        /// </summary>
        [Test]
        public void Test_IsSpriteFromUnity_Null()
        {
            // Arrange.
            TestDelegate action = () =>
            {
                // Act.
                SpriteUtility.IsSpriteFromUnity(null);
            };

            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is part of unity or not.
        /// It expects the return value to be true if the sprite is a unity sprite.
        /// </summary>
        [Test]
        public void Test_IsSpriteFromUnity_UnitySprite()
        {
            // Act.
            bool condition = SpriteUtility.IsSpriteFromUnity(_unitySprite);

            // Assert.
            Assert.IsTrue(condition);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is part of unity or not.
        /// It expects the value to be false if the sprite is a custom sprite.
        /// </summary>
        [Test]
        public void Test_IsSpriteFromUnity_CustomSprite()
        {
            // Act.
            bool condition = SpriteUtility.IsSpriteFromUnity(_customSprite);

            // Assert.
            Assert.IsFalse(condition);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is imported with sprite mode set to multiple.
        /// It expects a <see cref="ArgumentNullException"/> to be thrown if the sprite is null.
        /// </summary>
        [Test]
        public void Test_IsImportedWithMultipleSpriteMode_Null()
        {
            // Arrange.
            TestDelegate action = () =>
            {
                // Act.
                SpriteUtility.IsImportedWithMultipleSpriteMode(null);
            };

            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is imported with sprite mode set to multiple.
        /// Ite expects a value of true to be returned if the sprite uses a sprite mode set to multiple.
        /// </summary>
        [Test]
        public void Test_IsImportedWithMultipleSpriteMode_True()
        {
            // Act.
            bool condition = SpriteUtility.IsImportedWithMultipleSpriteMode(_customSpriteMultiple);

            // Assert.
            Assert.IsTrue(condition);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is imported with sprite mode set to multiple.
        /// It expects a value of false to be returned if the sprite doesn't use a sprite mode set to multiple.
        /// </summary>
        [Test]
        public void Test_IsImportedWithMultipleSpriteMode_False()
        {
            // Act.
            bool condition = SpriteUtility.IsImportedWithMultipleSpriteMode(_customSprite);

            // Assert.
            Assert.IsFalse(condition);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is part of an atlas or not.
        /// It expects a <see cref="ArgumentNullException"/> to be thrown if the sprite is null.
        /// </summary>
        [Test]
        public void Test_IsAtlasPacked_Null()
        {
            // Arrange.
            TestDelegate action = () =>
            {
                // Act.
                SpriteUtility.IsAtlasPacked(null);
            };

            // Assert.
            Assert.Catch<ArgumentNullException>(action);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is part of an atlas or not.
        /// It expects a value of true to be returned if the sprite is part of an atlas.
        /// </summary>
        [Test]
        public void Test_IsAtlasPacked_True()
        {
            // If sprite packager mode disabled this test fails because it can't be properly checked.
            if(EditorSettings.spritePackerMode == SpritePackerMode.Disabled)
                Assert.Fail("Sprite packer mode is set to disabled. Make sure it is enabled in Edit > ProjectSettings > Editor.");
            
            /* Note that this test will also fail if the custom sprite has not been packed recently. Make
             sure to pack the preview before running the test. */
            
            // Act.
            bool condition = SpriteUtility.IsAtlasPacked(_customSpriteAtlassed);

            // Assert.
            Assert.IsTrue(condition);
        }

        /// <summary>
        /// Tests whether the utility can return whether a sprite is part of an atlas or not.
        /// It expects a value of false to be returned if the sprite isn't part of an atlas.
        /// </summary>
        [Test]
        public void Test_IsAtlasPacked_False()
        {
            // Act.
            bool condition = SpriteUtility.IsAtlasPacked(_customSprite);

            // Assert.
            Assert.IsFalse(condition);
        }
    }
}

