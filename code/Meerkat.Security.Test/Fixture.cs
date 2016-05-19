using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using NCheck;
using NCheck.Checking;

using NUnit.Framework;

namespace Meerkat.Test
{
    public class Fixture
    {
        private ICheckerFactory checkerFactory;

        /// <summary>
        /// Gets the checker factory.
        /// </summary>
        /// <remarks>Creates it on first use and also assigns Checker.CheckerFactory</remarks>
        public ICheckerFactory CheckerFactory
        {
            get
            {
                if (checkerFactory == null)
                {
                    checkerFactory = CreateCheckerFactory();
                    if (checkerFactory == null)
                    {
                        throw new NotSupportedException("No CheckerFactory assigned to fixture");
                    }

                    // Set this as the global factory, needed by individual checkers if they do Entity checks
                    Checker.CheckerFactory = checkerFactory;
                }

                return checkerFactory;
            }
        }

        /// <summary>
        /// Pre-test set up.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            TidyUp();
            OnSetup();
        }

        /// <summary>
        /// Post-test tidy up
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            OnTearDown();
            TidyUp();
        }

        /// <summary>
        /// Add comparison to an entity checker.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        protected PropertyCheckExpression Compare<T>(Expression<Func<T, object>> propertyExpression)
        {
            return CheckerFactory.Compare(propertyExpression);
        }

        /// <summary>
        /// Verify that the state of a couple of objects is the same
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity we want to check</typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        protected void Check<TEntity>(TEntity expected, TEntity candidate)
        {
            Check(expected, candidate, typeof(TEntity).Name);
        }

        /// <summary>
        /// Verify that the state of a couple of objects is the same
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="objectName"></param>
        protected void Check<TEntity>(TEntity expected, TEntity candidate, string objectName)
        {
            CheckerFactory.Check(expected, candidate, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(IList<T> expectedList, IList<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(IList<T> expectedList, IList<T> candidateList, string objectName)
        {
            CheckerFactory.Check(expectedList as IEnumerable<T>, candidateList, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(ICollection<T> expectedList, ICollection<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(ICollection<T> expectedList, ICollection<T> candidateList, string objectName)
        {
            CheckerFactory.Check(expectedList as IEnumerable<T>, candidateList, objectName);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        protected void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList)
        {
            Check(expectedList, candidateList, string.Empty);
        }

        /// <summary>
        /// Verify that the contents of two lists of <see typeparamref="T" /> are the same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expectedList"></param>
        /// <param name="candidateList"></param>
        /// <param name="objectName"></param>
        protected void Check<T>(IEnumerable<T> expectedList, IEnumerable<T> candidateList, string objectName)
        {
            CheckerFactory.Check(expectedList, candidateList, objectName);
        }

        /// <summary>
        /// Verify that a comparison fails.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="name"></param>
        /// <param name="expectedValue"></param>
        /// <param name="actualValue"></param>
        protected void CheckFault<T>(T expected, T candidate, string name, object expectedValue, object actualValue)
        {
            CheckFault<T, Exception>(expected, candidate, name, expectedValue, actualValue);
        }

        /// <summary>
        /// Verify that a comparison fails.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException">Expected exception type</typeparam>  
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="name"></param>
        /// <param name="expectedValue"></param>
        /// <param name="actualValue"></param>
        protected void CheckFault<T, TException>(T expected, T candidate, string name, object expectedValue, object actualValue)
            where TException : Exception
        {
            const string MessageFormat = "{0}: Expected:<{1}>. Actual:<{2}>";

            var message = string.Format(MessageFormat, name, expectedValue, actualValue);

            CheckFault<T, TException>(expected, candidate, message);
        }

        /// <summary>
        /// Verify that a comparison fails
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="faultMessage"></param>
        protected void CheckFault<T>(T expected, T candidate, string faultMessage)
        {
            CheckFault<T, Exception>(expected, candidate, faultMessage);
        }

        /// <summary>
        /// Verify that a comparison fails
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException">Expected exception type</typeparam>        
        /// <param name="expected"></param>
        /// <param name="candidate"></param>
        /// <param name="faultMessage"></param>
        protected void CheckFault<T, TException>(T expected, T candidate, string faultMessage)
            where TException : Exception
        {
            try
            {
                Check(expected, candidate);
            }
            catch (TException ex)
            {
                Assert.AreEqual(faultMessage, ex.Message);
                return;
            }

            Assert.Fail("No exception, expected: " + faultMessage);
        }

        protected virtual ICheckerFactory CreateCheckerFactory()
        {
            return new CheckerFactory();
        }

        /// <summary>
        /// Test specific setup logic, should call base.OnSetup when used
        /// </summary>
        protected virtual void OnSetup()
        {
        }

        /// <summary>
        /// Test specific tear down logic.
        /// </summary>
        protected virtual void OnTearDown()
        {
        }

        /// <summary>
        /// Clear down the test context.
        /// </summary>
        protected virtual void TidyUp()
        {
            // Ensure that we wipe down the core objects - NUnit re-uses the instance for all tests
            checkerFactory = null;
        }
    }
}
