﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkovChainLib
{
    public class MarkovNode : IEquatable<MarkovNode>, IComparable<MarkovNode>
    {
        private NodeType nodeType;
        private string p;
        private string[] workingWords;

        public virtual string Prefix
        {
            get;
            set;
        }

        public virtual List<string> SuffixList
        {
            get;
            set;
        }

        public virtual NodeType Type
        {
            get;
            set;
        }

        public virtual MarkovChain MarkovChain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the tail of the markov node.
        /// </summary>
        /// <value>
        /// The tail, or the last string in the list of suffixes.
        /// </value>
        public virtual string Tail
        {
            get
            {
                return SuffixList[SuffixList.Count - 1];
            }
        }

        public MarkovNode(NodeType type, string prefix, params string[] suffixes)
        {
            Type = type;
            Prefix = prefix;
            SuffixList = new List<string>(suffixes);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(MarkovNode other)
        {
            // Get compare strings for two nodes
            var strThis = this.getCompareString();
            var strOther = other.getCompareString();

            // return the compare results
            return strThis.CompareTo(strOther);
        }

        /// <summary>
        /// Gets the compare string to be used in comparisons and equalities.
        /// </summary>
        /// <returns>String with prefix and all suffixes appended together, no spaces in between.</returns>
        private string getCompareString()
        {
            // Use a string builder to keep strings to a minimum
            var sb = new StringBuilder(this.Prefix.ToLower());

            // Add each suffix
            foreach (var item in this.SuffixList)
            {
                sb.Append(item.ToLower());
            }

            // Return string
            return sb.ToString();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(MarkovNode other)
        {
            // Get compare strings for two nodes
            var strThis = this.getCompareString();
            var strOther = other.getCompareString();

            // Check for equality
            return strThis.Equals(strOther);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return getCompareString().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // Create a hash code 4 characters long
            var strHashTail = GetHashCode().ToString().Substring(0, 4);
            var sb = new StringBuilder(strHashTail);

            // Attach the prefix
            sb.AppendFormat(": {0}", Prefix);

            // Attach all suffixes
            foreach (var suffix in SuffixList)
            {
                sb.AppendFormat(" {0}", suffix);
            }
            return sb.ToString();
        }
    }
}