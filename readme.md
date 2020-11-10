### Instructions
Read and parse one of the datasets at ./datasets. Your program should work correctly on all of the datasets

Write the list of users to the console in the following format:

```
UserId: e5f80f06-8261-4780-a887-e2e34a8e4194
Name: Jeff Woolley
Department: IT
Country: # Missing value
Age: # Value too high

UserId: 865ef167-da9f-4468-9bae-053cf70bad47
Name: Janice Barber
Age: 56
Work Schedule: Regular Daytime
Country: # Invalid value
```

Users have a series of attributes that are mapped to a list of attributes at the bottom of the datasets. The attribute list is dynamic (note that the attributes change between datasets), but you can assume the dataset formatting is always the same for parsing. 

If the attribute value is not valid according to the attribute rules (> MinValue, < MaxValue, !Optional and null, or not a valid option), output a validation error as in the example output.

Use any libraries that you find useful.
