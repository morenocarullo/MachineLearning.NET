using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.NET.RuleSystem
{
    /// <summary>
    /// Defines a set of rules that can be checked against an assignment.
    /// This assumes a disjunction of assertions only.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/06/29</date>
    public class RuleSet : IEnumerable<Rule>
    {
        private readonly List<Rule> m_ruleList;

        public RuleSet(IEnumerable<Rule> rules)
        {
            m_ruleList = rules.ToList();
        }

        public Rule this[int index]
        {
            get
            {
                return m_ruleList[index];
            }
        }

        public int Count
        {
            get
            {
                return m_ruleList.Count;
            }
        }

        #region IEnumerable<Rule> Members

        public IEnumerator<Rule> GetEnumerator()
        {
            return m_ruleList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(Rule rule)
        {
            m_ruleList.Add(rule);
        }

        public bool VerifiedBy(IDictionary<object, object> objectProperties)
        {
            foreach(var rule in m_ruleList)
            {
                if( objectProperties.ContainsKey(rule.Property) )
                {
                    var propertyValue = objectProperties[rule.Property];
                    if(!rule.VerifiedBy(propertyValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}