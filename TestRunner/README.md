# Testing SW2URDF

This project programmatically runs the tests of the SW2URDF project.
The tests rely on the models provided in the examples directory, so any changes to those files may cause these tests to fail.
Update any tests to reflect corresponding changes in the models.

## To Build

There is a test that checks that the git repo is not dirty, and all files have been committed.
To pass that test, you need to commit all files, then rebuild the solution.

When you build the solution, you should see two successful builds, SW2URDF and TestRunner.

## To Run

You can run tests directly from the Test Explorer in Visual Studio.
