/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : ThemeManager.cs                   '
'  Description      : Sets the bootstrap themes by      '
'                     taking the friendly name and      '
'                     converting it to a css file ref,  '
'                     defaults to bootstrap.css if no   '
'                     theme exists or the db value is   '
'                     incorrectly set.                  '
'  Author           : Brian McAulay                     '
'  Creation Date    : 18 Oct 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System;

namespace MVCWebProject2.utilities
{
    public static class ThemeManager
    {
        public static string SetThemeName(string themeName)
        {
            foreach (Constants.BootstrapThemes theme in Enum.GetValues(typeof(Constants.BootstrapThemes)))
            {
                if (theme.ToString().ToLower() == themeName.ToLower())
                {
                    return theme.GetStringValue();
                }
            }
            return "bootstrap.css";

        }
    }
}