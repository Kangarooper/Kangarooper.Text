﻿using System;

namespace SoftwareBotany.Ivy
{
    public static partial class StringSchemaExtensions
    {
        /// <summary>
        /// Splits a row, string, using the provided StringSchema. Exactly one Entry must be found in the Schema
        /// for which the row StartsWith the Entry's Header. This Entry then provides Width and FillCharacter
        /// parameters needed to perform the Split.
        /// </summary>
        /// <param name="row">The row, string, to Split after finding its StringSchemaEntry.</param>
        /// <param name="schema">The StringSchema containing exactly one Entry for whom this row StartsWith the Entry's Header.</param>
        /// <example>
        /// <code>
        /// var schema = new StringSchema(new StringSchemaEntry[]
        /// {
        ///     new StringSchemaEntry("A", new[] { 1, 1, 1 }),
        ///     new StringSchemaEntry("B", new[] { 2, 2, 2 }),
        ///     new StringSchemaEntry("CD", new[] { 3, 3, 3 })
        /// });
        ///
        /// var split = "A123".SplitSchemaRow(schema);
        /// Console.WriteLine(split.Entry.Header);
        /// foreach(string column in split)
        ///     Console.WriteLine(column);
        ///
        /// split = "B123456".SplitSchemaRow(schema);
        /// Console.WriteLine(split.Entry.Header);
        /// foreach(string column in split)
        ///     Console.WriteLine(column);
        ///
        /// split = "CD123456789".SplitSchemaRow(schema);
        /// Console.WriteLine(split.Entry.Header);
        /// foreach(string column in split)
        ///     Console.WriteLine(column);
        /// </code>
        /// Console Output:
        /// <code>
        /// A
        /// 1
        /// 2
        /// 3
        /// B
        /// 12
        /// 34
        /// 56
        /// CD
        /// 123
        /// 456
        /// 789
        /// </code>
        /// </example>
        public static StringSchemaEntryAndColumns SplitSchemaRow(this string row, StringSchema schema)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            if (schema == null)
                throw new ArgumentNullException("schema");

            StringSchemaEntry entry = schema.GetEntryForRow(row);

            using (var charEnumerator = row.Substring(entry.Header.Length).GetEnumerator())
            {
                return new StringSchemaEntryAndColumns(entry, StringFixedExtensions.SplitFixedImplementation(charEnumerator, entry.Widths, entry.FillCharacter));
            }
        }
    }
}