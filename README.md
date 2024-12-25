# CssDeDup

Supports CSS file deduping, common style rule extraction and more (more detailed description below).

CssDeDup depends on [ExCSS](https://github.com/TylerBrinks/ExCSS) to parse its CSS documents, allowing CssDeDup to work its magic. A big thanks to Tyler Brinks
for writing ExCSS.


# Table Of Contents

- [Detailed explaination](README.md#detailed-explaination)  
  - [Terminology](README.md#terminology)  
  - [Functions](README.md#functions)  
     - [A. Exact Matching](README.md#a-exact-matching)  
     - [B. CSS Rule Name Wildcard Matching](README.md#b-css-rule-name-wildcard-matching)  
     - [C. Fuzzy Property Value Matching](README.md#c-fuzzy-property-value-matching)  
- [Usage Examples](README.md#usage-examples)  


# Detailed explaination

## Terminology
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

## Functions

CssDeDup has 3 different Functions:
- [A. Exact Matching](README.md#a-exact-matching) (CSS Rule Name & Properties must match exactly)
- [B. CSS Rule Name Wildcard Matching](README.md#b-css-rule-name-wildcard-matchingg) (Only properties must match; CSS rule name can be anything)
- (Planned) [C. Fuzzy Property Value Matching](README.md#c-fuzzy-property-value-matching); NOTE: Not implemented yet.

All three features take two css documents as input.

It parses them into two collections of css rules. 

It then finds "matching" css rules and extracts any css rules it finds to be a match into a third CSS document.



### A. Exact Matching
This considers two css rules a match if they match ALL 3 of the following criteria:
1) The rule name
2) The set of properties being set
3) The property values

So the css rule would have to be basically a copy-paste-identical match to be considered a match (minus case, whitespace and non-functional differences).

### B. CSS Rule Name Wildcard Matching

This does #2 and #3 from the list above, but ignores #1. 

The CSS rule name can be anything.

### C. Fuzzy Property Value Matching

This will allow fuzzy matching on the property values being set. 

Examples of fuzzy matching on CSS property values:
- Two px values are within +-3 px of each other.
- Two colors are perceptually near-identical.


# Usage Examples

`CssDeDup.exe nameandproperties --in1="C:\inetpub\wwwroot\NewsWebsite\default.css" --in2="C:\inetpub\wwwroot\AdminWebsite\default.css" --out="C:\Temp\CSS\CommonStyles_News-Admin.css"`
- Option `nameandproperties` performs function [**A. Exact Matching**](README.md#a-exact-matching) from above.
- Takes 3 arguments: Two in files (--in1 & --in2) and one out file (--out)



`CssDeDup.exe properties --in1="C:\inetpub\wwwroot\AdminWebsite\default.css" --in2="C:\inetpub\wwwroot\Old_AdminWebsite\default.css" --out="C:\Temp\CSS\CommonStyles_Admin-OldAdmin.css"`
- Option `properties` performs function [**B. CSS Rule Name Wildcard Matching**](README.md#b-css-rule-name-wildcard-matchingg) from above.
- Takes 3 arguments: Two in files (--in1 & --in2) and one out file (--out)










