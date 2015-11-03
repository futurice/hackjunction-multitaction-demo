HackJunction 2015 Cornerstone SDK README
========================================
Cornerstone SDK is a true multi-touch, multi-user C++ Widget-toolkit. More info and documentation: https://cornerstone.multitouch.fi/developer_guide

### Cornerstone installation instructions
Go to Downloads after registering and logging in. Installers are available at the demo site also.

### Display configuration
When using with MultiTaction displays, you will need to add the IP address of the display to the configuration config.txt. This file by default is found in %APPDATA%/MultiTouch or ~/.MultiTouch. After this all Cornerstone applications just work.

`NetBridge {
  host = "ip-address-of-the-display"
}`

Path to the file can also be given as an application argument: `--config <filepath>`

#### Windows touch or TUIO 
You can also develop application using any other toolkit. When using Windows Touch, just run *WindowsTouchProxy* which uses the same config.txt. For TUIO setup (f.ex when using Unity) either add *TuioSender* block to config.txt or enter the IP address of your TUIO listener to the MultiTaction OSD (On Screen Display). After this you need to run *MTServer* executable which passes the events from the display(s) to the application.

TUIOSender {  
  features = "fingers objects"  
  /* address: The TUIO stream receiver IP network address, default = 127.0.0.1 */  
  address = "localhost"  
  /* port: The TUIO stream receiver IP port, default = 3333 */  
  port = "3333"  
}

Compile instructions
--------------------
Follow the instructions for installing the toolchain: https://cornerstone.multitouch.fi/developer-guide/setup.html

GCC or VS2012, qmake is included in the SDK. Ubuntu 14.04 is also supported. 

### Windows
Use QT Creator and it just works, or create VS project files by running this in the cmd:  
`qmake -tp vc`

After succesful build, add <CornerstoneInstallDir>/bin to the PATH for running and debugging.  
`Properties > Debugging > Environment: Path=$(CORNERSTONE_ROOT_2_0_8)/bin;$(Path)`


### Linux
For Ubuntu 12.04:  
`qmake`  
`make`

and for Ubuntu 14.04:  
`qmake-qt4`       
`make`

### OS X
see https://cornerstone.multitouch.fi/developer-guide/setup.html

### Tools and best practices
#### Widget Inspector
You can easily see and edit all the widgets and their attributes from the *WidgetInspector*. Pressing 'i' on the keyboard will invoke it. You can also use it for adding and serializing them for quick sketching

#### Virtual input
It is useful to have some virtual input method while developing, since you don't always have a MultiTaction display available.

Normal finger interaction works with mouse. Pressing 'p' on keyboard will create a virtual finger that allows scaling and rotating of widgets. Pressing 'y' will toggle the input method from *fingers* to *ir-pen* to *marker* and back to *fingers*

#### CSS
Cornerstone has a built-in CSS parser which allows you to use CSS for changing the layout, but also all the attributes of the Widgets. See Widget class documentation (or WidgetInspector) for all possible attributes for each widget. F.ex find all ImageWidget attributes under *Valuable Attributes* in https://cornerstone.multitouch.fi/developer-guide/class_multi_widgets_1_1_image_widget.html


