using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcNugetTool.Service;

namespace GrpcNugetTool
{
    public class DynamicMenuCommand:OleMenuCommand
    {
        private Predicate<int> _matches;
        private int _matchedCommandId;

        public const string guidDynamicMenuPackageCmdSet = "b9378778-4265-475f-a0f0-13546abcb9cb";
        public const int cmdidMyCommand = 0x1030;
        public const int cmdidMyCommandSub1 = 0x0103;
        public const int cmdidMyCommandSub2 = 0x0104;

        public DynamicMenuCommand(
            CommandID rootId, 
            Predicate<int> matches, 
            EventHandler invokeHandler, 
            EventHandler beforeQueryStatusHandler)
            : base(invokeHandler, null /*changeHandler*/, beforeQueryStatusHandler, rootId)
        {
            _matches = matches ?? throw new ArgumentNullException("matches");
        }

        public override bool DynamicItemMatch(int cmdId)
        {
            if(_matches(cmdId))
            {
                _matchedCommandId = cmdId;
                return true;
            }
            _matchedCommandId = 0;
            return false;
        }
        
        
    }

    class DynamicMenu
    {
        private DTE2 _dte2;
        private Package _package;
        public DynamicMenu(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            _package = package;

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                // Add the DynamicItemMenuCommand for the expansion of the root item into N items at run time.
                CommandID dynamicItemRootId = new CommandID(new Guid(DynamicMenuCommand.guidDynamicMenuPackageCmdSet), DynamicMenuCommand.cmdidMyCommand);
                DynamicMenuCommand dynamicMenuCommand = new DynamicMenuCommand(dynamicItemRootId,
                    IsValidDynamicItem,
                    OnInvokedDynamicItem,
                    OnBeforeQueryStatusDynamicItem);
                commandService.AddCommand(dynamicMenuCommand);

                CommandID dynamicItemRootId4 = new CommandID(new Guid(DynamicMenuCommand.guidDynamicMenuPackageCmdSet), DynamicMenuCommand.cmdidMyCommandSub1);
                DynamicMenuCommand dynamicMenuCommand4 = new DynamicMenuCommand(dynamicItemRootId4,
                    IsValidDynamicItem,
                    OnInvokedDynamicItem,
                    null);
                commandService.AddCommand(dynamicMenuCommand4);

                CommandID dynamicItemRootId6 = new CommandID(new Guid(DynamicMenuCommand.guidDynamicMenuPackageCmdSet), DynamicMenuCommand.cmdidMyCommandSub2);
                DynamicMenuCommand dynamicMenuCommand6 = new DynamicMenuCommand(dynamicItemRootId6,
                    IsValidDynamicItem,
                    OnInvokedDynamicItem,
                    null);
                dynamicMenuCommand6.Text = "a";
                commandService.AddCommand(dynamicMenuCommand6);
            }

            _dte2 = (DTE2)this.ServiceProvider.GetService(typeof(DTE));
        }

        private System.IServiceProvider ServiceProvider
        {
            get
            {
                return _package;
            }
        }

        private bool IsValidDynamicItem(int commandId)
        {
            // The match is valid if the command ID is >= the id of our root dynamic start item
            // and the command ID minus the ID of our root dynamic start item
            // is less than or equal to the number of projects in the solution.
            return commandId >= DynamicMenuCommand.cmdidMyCommandSub1 && commandId<=DynamicMenuCommand.cmdidMyCommandSub2;
        }
        private void OnInvokedDynamicItem(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            OleMenuCommand menuCommand=null;
            try
            {
                menuCommand = sender as OleMenuCommand;
                if (menuCommand != null)
                {
                    switch (menuCommand.CommandID.ID)
                    {
                        case DynamicMenuCommand.cmdidMyCommandSub1:
                            VsShellUtilities.ShowMessageBox(
                                _package, 
                                Util.GetVsixCurrentDirectory(), 
                                "GrpcClient Exception",
                                OLEMSGICON.OLEMSGICON_INFO,
                                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                            var service1 = new GrpcGenerateService(_dte2.ActiveDocument.FullName,false);
                            service1.Run(true);
                            break;
                        case DynamicMenuCommand.cmdidMyCommandSub2:
                            var service2 = new GrpcGenerateService(_dte2.ActiveDocument.FullName,true);
                            service2.Run(true);
                            break;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                string message = $"{menuCommand?.CommandID.ID??null}:{ex.Message}";
                VsShellUtilities.ShowMessageBox(
                    _package,
                    message,
                    "GrpcClient Exception",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }
        private void OnBeforeQueryStatusDynamicItem(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            OleMenuCommand menuCommand = sender as OleMenuCommand;
            if (menuCommand != null)
            {
                string fileName=_dte2.ActiveDocument.FullName;
                if (fileName.EndsWith(".proto"))
                    menuCommand.Visible = true;
                else
                    menuCommand.Visible = false;
            }
        }
    }
}
