
# BizUnit Designer V2

## Introduction
BizUnit Designer is a graphical interface originally created by [Praneeth Reddy](https://www.codeplex.com/site/users/view/praneethreddy) to allow for the creation of [BizUnit](http://bizunit.codeplex.com/) test cases for unit testing or system testing distributed applications.  

The original version of BUD was designed to work with BizUnit 2.x and was hosted on [CodePlex](https://bud.codeplex.com/) and works well for older versions of BizTalk.  

However, the [BizUnit framework](http://bizunit.codeplex.com/) used by the BizUnit designer has been updated.  With this in mind, it is time to update the BizUnit Designer to work with this updated version of the BizUnit Framework.
This repository contains a version of BizUnit Designer which uses version 4.1 of the BizUnit Framework and is compiled against version 4.5 of the Dot Net Framework.

**Note: As my colleagues and I've not been able to reach the original authors of BUD to include this update in their CodePlex repository, I've chosen to host the update on GitHub.**

Please refer to the [readme.pdf](../master/BizUnitDesigner/BizUnitDesigner/bin/Release/readme.pdf) file for information on how to use this designer.  Also take a look at [Kevin Smith's blog](https://kevinsmi.wordpress.com/?s=bizunit) for information on BizUnit.

I have **not** extensively tested this version of BUD, so if you encounter any issues, please either message me via GitHub or submit a pull request.

## Credits
The BizUnit Framework was created by Kevin B. Smith and is available to download from GitHub at <http://bizunit.codeplex.com/>

BizUnit Designer was originally created by Praneeth Reddy <https://bud.codeplex.com/> and the CodePlex repository hosts a [FAQ](https://bud.codeplex.com/wikipage?title=faq&referringTitle=Home) section which is useful when getting started.

## Points to Note
If you download this source code, you'll also need to download the [BizUnit framework](http://bizunit.codeplex.com/), and then build and GAC the *BizUnit.dll*.

BizUnit Designer contains a vdproj file that allows for the creation of a setup.exe file. 
Visual Studio 2013 supports an extension that works with the vdproj file and allows for the creation on a setup.exe file.
More info on this extension and how to install it can be found here <https://blogs.msdn.microsoft.com/buckh/2011/03/17/visual-studio-setup-projects-vdproj-will-not-ship-with-future-versions-of-vs/>.

Alternatively, you can download the [setup.exe file](https://github.com/mikehowell/BUD/releases/download/2.0/setup.exe) if you are interested in just installing and working with BizUnit Designer.

