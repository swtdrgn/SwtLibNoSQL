using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwtLib
{
    public static class NoSQLComparison
    {
        public enum Operator { EQ, NEQ, LT, LTE, GT, GTE };
    }

    public interface NoSQLCondition
    {
        string VariableName { get; }
        NoSQLComparison.Operator ComparisonOperator { get; }
        dynamic Value { get; }
    }

    public class NoSQLCondition<T> : NoSQLCondition
    {
        string _variableName;
        NoSQLComparison.Operator _op;
        T _value;

        private NoSQLCondition() { }
        public NoSQLCondition(string variableName, NoSQLComparison.Operator comparisonOperator, T value)
        {
            _variableName = variableName;
            _op = comparisonOperator;
            _value = value;
        }

        public string VariableName { get { return _variableName; } }
        public NoSQLComparison.Operator ComparisonOperator { get { return _op; } }
        dynamic NoSQLCondition.Value { get { return _value; } }

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
