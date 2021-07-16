Very much a WIP and experimentation bit of coding...

The intention is to be able to create a Blazor Edit Form from a JSON file, with validation...etc
The main problem is generating the class/model from the json with the appropiate attributes. This is done using reflection.emit to create the class during runtime, in memory.
This seems to be the better method over ExpandoObjects, which is quite limited is a bit rubiish for boxing/unboxing all over the place for this use case and codedom wasn't quite the right solution either.

The actual generation of the form is done by a library called VxFormGenerator.
I had mostly done it myself, but this included all the layout stuff and seems to work as I want, saved me a job for the time being.
