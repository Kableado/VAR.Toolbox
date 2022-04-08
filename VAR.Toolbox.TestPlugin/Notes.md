## Preparation

* Build as class library.
* Add reference to ```VAR.Toolbox``` for interface declarations and utilities.
* Use the interfaces to extend functionality:
    * ```IToolForm```: Tool window.
    * ```IToolPanel```: Tool panels in the main window. Try to fit all on 200px width.
    * ```ITextCoder```: Text codification clases.
    * ```IProxyCmdExecutor```: Proxy command executors, mainly for remote executions.

## Usage

Put the generated assembly near ```VAR.Toolbox.exe``` it will load as plug-ins any assembly that starts
with ```VAR.Toolbox```