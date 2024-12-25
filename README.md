# CssDeDup

Supports CSS file deduping, common style rule extraction and more (more detailed description below).

CssDeDup depends on [ExCSS](https://github.com/TylerBrinks/ExCSS) to parse its CSS documents, allowing CssDeDup to work its magic. A big thanks to Tyler Brinks
for writing ExCSS.


# Detailed explaination

First, to help me explain things, lets define some terminology first:

```
.danger-text {
   color: red;
}
```

The text above is a CSS style **'rule'**.

"danger-text" is the **'rule name'**.

"color" is the **'property'** 

"red" is the **'property value'**.


. . .



CssDeDup has 3 different workflows:
- [Exact Matching](README.md#exact-matching) (CSS Rule Name & Properties must match exactly)
- [CSS Rule Name Wildcard Matching](README.md#css-rule-name-wildcard-matching) (Only properties must match; CSS rule name can be anything)
- (Planned) [Fuzzy Property Value Matching](README.md#fuzzy-property-value-matching); NOTE: Not implemented yet.

All three features take two css documents as input.

It parses them into two collections of css rules. 

It then finds "matching" css rules and extracts any css rules it finds to be a match into a third CSS document.



## Exact Matching
This considers two css rules a match if they match ALL 3 of the following criteria:
1) The rule name
2) The set of properties being set
3) The property values

So the css rule would have to be basically a copy-paste-identical match to be considered a match (minus case, whitespace and non-functional differences).

## CSS Rule Name Wildcard Matching

This does #2 and #3 from the list above, but ignores #1. 

The CSS rule name can be anything.

## Fuzzy Property Value Matching

This will allow fuzzy matching on the property values being set. 

Examples of fuzzy matching on CSS property values:
- Two px values are within +-3 px of each other.
- Two colors are perceptually near-identical.

