using NUnit.Framework;
using System;
using DTT.Utils.EditorUtilities.Extensions;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Tests
{
    /// <summary>
    /// Test the <see cref="SerializedPropertyExtensions"/>
    /// </summary>
    public class Test_SerializablePropertyExtensions
    {
        /// <summary>
        /// The serializable scriptable object used for testing.
        /// </summary>
        [Serializable]
        public class ObjectWithArray : ScriptableObject
        {
            public string[] values = Array.Empty<string>();

            public string sibling = string.Empty;

            public const string SIBLING_NAME = nameof(sibling);
        }

        /// <summary>
        /// The name of the values property.
        /// </summary>
        private const string ARRAY_PROPERTY_NAME = "values";

        /// <summary>
        /// The string test value.
        /// </summary>
        private const string STRING_TEST_VALUE = "value";

        /// <summary>
        /// Tests whether a sibling property can be returned.
        /// It expects the sibling property to be returned if the name corresponds.
        /// </summary>
        [Test]
        public void Test_GetSiblingProperty()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            
            // Act.
            SerializedProperty property = array.GetSiblingProperty(ObjectWithArray.SIBLING_NAME);
            
            // Assert.
            Assert.IsNotNull(property, "Expected the sibling to be found but it wasn't.");
        }
        
        /// <summary>
        /// Tests whether a sibling property can be returned.
        /// It expects an exception if the sibling property is null.
        /// </summary>
        [Test]
        public void Test_GetSiblingProperty_Null_Property()
        {
            // Arrange.
            SerializedProperty array = null;
            
            // Act.
            TestDelegate action = () => array.GetSiblingProperty(ObjectWithArray.SIBLING_NAME);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null property to cause an exception but it didn't.");
        }
        
        /// <summary>
        /// Tests whether a sibling property can be returned.
        /// It expects an exception if the property name is null.
        /// </summary>
        [Test]
        public void Test_GetSiblingProperty_Null_PropertyName()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            
            // Act.
            TestDelegate action = () => array.GetSiblingProperty(null);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null property name to cause an exception but it didn't.");
        }

        /// <summary>
        /// Tests whether an array element can be detected.
        /// It expects the array element to be detected if it is one.
        /// </summary>
        [Test]
        public void Test_isArrayElement_True()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            
            array.AddArrayElement(STRING_TEST_VALUE);
            SerializedProperty element = array.GetArrayElementAtIndex(0);

            // Act.
            bool condition = element.IsArrayElement();
            
            // Assert.
            Assert.IsTrue(condition, "Expected the property to be an array element but it wasn't.");
        }
        
        /// <summary>
        /// Tests whether an array element can be detected.
        /// It expects the return value false if the property is not an array value.
        /// </summary>
        [Test]
        public void Test_isArrayElement_False()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ObjectWithArray.SIBLING_NAME);

            // Act.
            bool condition = array.IsArrayElement();
            
            // Assert.
            Assert.IsFalse(condition, "Expected the property not to be an array element but it was.");
        }
        
        /// <summary>
        /// Tests whether an array element can be detected.
        /// It expects an exception if the property is null.
        /// </summary>
        [Test]
        public void Test_isArrayElement_Null_Property()
        {
            // Arrange.
            SerializedProperty array = null;

            // Act.
            TestDelegate action = () => array.IsArrayElement();
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null property to cause an exception but it didn't.");
        }
        
        /// <summary>
        /// Tests an array element can be removed.
        /// It expected the element to be removed if property is in the array.
        /// </summary>
        [Test]
        public void Test_RemoveArrayElement_In_Array()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);

            array.AddArrayElement(STRING_TEST_VALUE);
            
            // Act.
            SerializedProperty property = array.GetArrayElementAtIndex(0);
            array.RemoveArrayElement(property);
            
            // Assert.
            Assert.Zero(array.arraySize, "Expected the array size to be zero after removal but it wasn't.");
        }
        
        /// <summary>
        /// Tests an array element can be removed.
        /// It expects the array size not to change if the property wasn't in the array.
        /// </summary>
        [Test]
        public void Test_RemoveArrayElement_Not_In_Array()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            SerializedProperty sibling = serializedObject.FindProperty(ObjectWithArray.SIBLING_NAME);

            array.AddArrayElement(STRING_TEST_VALUE);
            
            // Act.
            array.RemoveArrayElement(sibling);
            
            // Assert.
            Assert.NotZero(array.arraySize, "Expected the array size not to be zero after removal but it was.");
        }
        
        /// <summary>
        /// Tests an array element can be removed.
        /// It expects an exception if the property is null.
        /// </summary>
        [Test]
        public void Test_RemoveArrayElement_Null_Property()
        { // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = null;
            SerializedProperty sibling = serializedObject.FindProperty(ObjectWithArray.SIBLING_NAME);
            
            // Act.
            TestDelegate action = () => array.RemoveArrayElement(sibling);

            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null array to cause an exception but it didn't.");
        }

        /// <summary>
        /// Tests whether an array element can be added.
        /// It expects a string element to be added.
        /// </summary>
        [Test]
        public void Test_AddArrayElement_String()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);

            // Act.
            array.AddArrayElement(STRING_TEST_VALUE);
            
            // Assert.
            Assert.NotZero(array.arraySize, "Expected an element to be added but there wasn't.");
            Assert.AreEqual(array.GetArrayElementAtIndex(0).stringValue, STRING_TEST_VALUE,
                "Expected the string value to be set but it wasn't.");
        }
        
        /// <summary>
        /// Tests whether an array element can be inserted.
        /// It expects a string element to be inserted at a given index added.
        /// </summary>
        [Test]
        public void Test_AddArrayElement_Insert()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            array.AddArrayElement(STRING_TEST_VALUE);

            // Act.
            string newValue = STRING_TEST_VALUE + "_NEW";
            array.AddArrayElement(newValue, 0);
            
            // Assert.
            Assert.AreEqual(array.GetArrayElementAtIndex(0).stringValue, newValue,
                "Expected the string value to be inserted but it wasn't.");
        }

        /// <summary>
        /// Tests whether an array can return whether it contains an element.
        /// It expects true to be returned if the element is in the array.
        /// </summary>
        [Test]
        public void Test_ContainsArrayElement_True()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            
            array.AddArrayElement(STRING_TEST_VALUE);
            
            // Act.
            SerializedProperty property = array.GetArrayElementAtIndex(0);
            bool condition = array.ContainsArrayElement(property);
            
            // Assert.
            Assert.IsTrue(condition, "Expected the array to contain the value after adding it but it didn't.");
        }
        
        /// <summary>
        /// Tests whether an array can return whether it contains an element.
        /// It expects false to be returned if the element is not in the array.
        /// </summary>
        [Test]
        public void Test_ContainsArrayElement_False()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            SerializedProperty sibling = serializedObject.FindProperty(ObjectWithArray.SIBLING_NAME);
            
            array.AddArrayElement(STRING_TEST_VALUE);
            
            // Act.
            bool condition = array.ContainsArrayElement(sibling);
            
            // Assert.
            Assert.IsFalse(condition, "Expected the array to not contain the value after adding it but it did.");
        }
        
        /// <summary>
        /// Tests whether an array can return whether it contains an element.
        /// It expects an exception if the property is null.
        /// </summary>
        [Test]
        public void Test_ContainsArrayElement_Null_Property()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = null;
            SerializedProperty sibling = serializedObject.FindProperty(ObjectWithArray.SIBLING_NAME);

            TestDelegate action = () => array.ContainsArrayElement(sibling);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null array to cause an exception but it didn't.");
        }
        
        /// <summary>
        /// Tests whether an array can return whether it contains an element.
        /// It expects an exception if the value to check is null.
        /// </summary>
        [Test]
        public void Test_ContainsArrayElement_Null_Value()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);

            // Act.
            TestDelegate action = () => array.ContainsArrayElement(null);
            
            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null array to cause an exception but it didn't.");
        }

        /// <summary>
        /// Tests whether an index of an element can be found.
        /// It expects the index to be returned if the condition was correct.
        /// </summary>
        [Test]
        public void Test_FindIndexInArray_Found()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            
            array.AddArrayElement(STRING_TEST_VALUE);
            
            // Act.
            int actual = array.FindIndexInArray((property) => property.stringValue == STRING_TEST_VALUE);
            int expected = 0;
            
            // Assert.
            Assert.AreEqual(expected, actual, "Expected the index to be found but it wasn't.");
        }
        
        /// <summary>
        /// Tests whether an index of an element can be found.
        /// It expects the index to be returned if the condition was not correct.
        /// </summary>
        [Test]
        public void Test_FindIndexInArray_Not_Found()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);

            // Act.
            int actual = array.FindIndexInArray((property) => property.stringValue == STRING_TEST_VALUE);
            int expected = -1;
            
            // Assert.
            Assert.AreEqual(expected, actual, "Expected the index not to be found but it was.");
        }
        
        /// <summary>
        /// Tests whether an index of an element can be found.
        /// It expects an exception if the property was null.
        /// </summary>
        [Test]
        public void Test_FindIndexInArray_Null_Property()
        {
            // Arrange.
            SerializedProperty array = null;

            // Act.
            TestDelegate action = () => array.FindIndexInArray((property) => property.stringValue == STRING_TEST_VALUE);

            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null array to cause an exception but it didn't.");
        }
        
        /// <summary>
        /// Tests whether an index of an element can be found.
        /// It expects an exception if the condition was null.
        /// </summary>
        [Test]
        public void Test_FindIndexInArray_Null_Condition()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);

            // Act.
            TestDelegate action = () => array.FindIndexInArray(null);

            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null condition to cause an exception but it didn't.");
        }

        /// <summary>
        /// Tests whether array values can be returned.
        /// It expects the array values to be equal in length to the property.
        /// </summary>
        [Test]
        public void Test_GetArrayValues()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            
            array.AddArrayElement(STRING_TEST_VALUE);
            
            // Act.
            SerializedProperty[] values = array.GetArrayValues();
            
            // Assert.
            Assert.AreEqual(values.Length, array.arraySize, "Expected the values to be returned but they weren't.");
        }
        
        /// <summary>
        /// Tests whether array values can be returned.
        /// It expects an exception if the property was null.
        /// </summary>
        [Test]
        public void Test_GetArrayValues_Null_Property()
        {
            // Arrange.
            SerializedProperty array = null;

            // Act.
            TestDelegate action = () => array.FindIndexInArray(null);

            // Assert.
            Assert.Catch<ArgumentNullException>(action, "Expected the null property to cause an exception but it didn't.");
        }

        /// <summary>
        /// Tests whether the array size can be increased.
        /// It expects the array size to increase after the method has called.
        /// </summary>
        [Test]
        public void Test_IncreaseArraySize_No_Insert_Index()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);

            int arraySize = array.arraySize;

            // Act.
            array.IncreaseArraySize();
            
            // Assert.
            Assert.AreEqual(arraySize + 1, array.arraySize, "Expected the size to be increased but it wasn't.");
        }
        
        /// <summary>
        /// Tests whether the array size can be increased.
        /// It expects the empty element to be inserted at index if an index is given.
        /// </summary>
        [Test]
        public void Test_IncreaseArraySize_Insert_Index()
        {
            // Arrange.
            ObjectWithArray objectWithArray = ScriptableObject.CreateInstance<ObjectWithArray>();
            SerializedObject serializedObject = new SerializedObject(objectWithArray);
            SerializedProperty array = serializedObject.FindProperty(ARRAY_PROPERTY_NAME);
            
            array.AddArrayElement(string.Empty);
            array.AddArrayElement(STRING_TEST_VALUE);

            // Act.
            array.IncreaseArraySize(0);
            string value = array.GetArrayElementAtIndex(0).stringValue;
            
            // Assert.
            Assert.AreEqual(string.Empty, value, "Expected the element to be inserted at index 0 but it wasn't.");
        }
    }
}
