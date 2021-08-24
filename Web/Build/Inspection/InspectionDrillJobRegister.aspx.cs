using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Registers_DrillJobRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillJobRegister.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDrillJoblist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        Tabstripdrilljobregistermenu.MenuList = toolbargrid.Show();

        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Drill Log", "Toggle4", ToolBarDirection.Right);
        menu.AddButton("History", "Toggle3", ToolBarDirection.Right);
        menu.AddButton("Drill Schedule", "Toggle2", ToolBarDirection.Right);
        menu.AddButton("Drill Job Register", "Toggle1", ToolBarDirection.Right);
       

        Tabstripdrilljobregister.MenuList = menu.Show();

        Tabstripdrilljobregister.SelectedMenuIndex = 3;

        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
           
            ViewState["PAGENUMBER"] = 1;

            int? vesselid = General.GetNullableInteger(Request.QueryString["vesselid"]);
            if (vesselid == null)
            {
                ViewState["VESSELID"] = null;

            }
            else
            {
                ViewState["VESSELID"] = vesselid;
                ddlvessellist.SelectedVessel = vesselid.ToString();
            }
            gvDrillJoblist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }


        

    }

    protected void ShowExcel()
    {
        int         iRowCount       = 0;
        int         iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE",  "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDAFFECTEDBYCREWCHANGEYN", "FLDCREWCHANGEPERCENTAGE", "FLDPHOTOYN", "FLDDASHBOARDYN" };
        string[] alCaptions = { "Name", "Interval", "Interval Type",  "Fixed/Variable", "Type", "Affected by Crew Change?", "Crew Change Percentage", "Photo Mandatory (Y/N)", "Show in Dashboard (Y/N)" };


        string vesselname      = string.Empty;



        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount       = 10;
        else
                    iRowCount       = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        int?        vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());

        DataTable   dt              = PhoenixInspectionDrillJob.drilljoblist(vesselid, gvDrillJoblist.CurrentPageIndex + 1,
                                               gvDrillJoblist.PageSize,
                                               ref iRowCount,
                                               ref iTotalPageCount,
                                               ref vesselname);


        Response.AddHeader("Content-Disposition", "attachment; filename=Drill Job Register.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill Job Register</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("<td> Vessel Name = '" + vesselname + "'</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void drilljobregistermenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce         = (RadToolBarEventArgs)e;
            string              CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }

    }


    protected void drilljobregister_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs     dce         = (RadToolBarEventArgs)e;
            string                  CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("TOGGLE1"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillJobRegister.aspx?vesselid=" + vesselid );
                }

                else
                    Response.Redirect("~/Inspection/InspectionDrillJobRegister.aspx");
            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillHistory.aspx?vesselid=" + vesselid);
                }
                else 

                Response.Redirect("~/Inspection/InspectionDrillHistory.aspx");
               
               
            }
            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillSchedule.aspx?vesselid=" + vesselid);
                }
                else 

                Response.Redirect("~/Inspection/InspectionDrillSchedule.aspx");

                
                
            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillReport.aspx?vesselid=" + vesselid);
                }
                else 


                Response.Redirect("~/Inspection/InspectionDrillReport.aspx");

            }
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }
    }


    
    protected void ddlvessellist_textchange(object sender, EventArgs e)
    {
        gvDrillJoblist.Rebind();

        if (ddlvessellist.SelectedVessel != null)
        {

            ViewState["VESSELID"] = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        }
       

    }
    protected void gvDrillJoblist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int         iRowCount       = 0;
        int         iTotalPageCount = 0;
        string      vesselname      = string.Empty;
        string[] alColumns = { "FLDDRILLNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDAFFECTEDBYCREWCHANGEYN", "FLDCREWCHANGEPERCENTAGE", "FLDPHOTOYN", "FLDDASHBOARDYN" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Affected by Crew Change?", "Crew Change Percentage", "Photo Mandatory (Y/N)", "Show in Dashboard (Y/N)" };


        int?        vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());

        DataTable   dt              = PhoenixInspectionDrillJob.drilljoblist(vesselid, gvDrillJoblist.CurrentPageIndex + 1,
                                                gvDrillJoblist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                ref vesselname);

        gvDrillJoblist.DataSource           = dt;
        gvDrillJoblist.VirtualItemCount     = iRowCount;
       
        DataSet ds = new DataSet();
        ds = dt.DataSet;
       

        General.SetPrintOptions("gvDrillJoblist", "Drill Job Register", alCaptions, alColumns, ds);


    }

    protected void crewchangeeffect(object sender, EventArgs e)
    {

        RadCheckBox checkbox = sender as RadCheckBox;
        GridEditableItem item = (checkbox.NamingContainer as GridEditableItem);


        UserControlMaskNumber crewchangepercentageedit = (UserControlMaskNumber)item.FindControl("tbcrewpercentedit");
        UserControlMaskNumber crewchangepercentageeditdummy = (UserControlMaskNumber)item.FindControl("MaskNumber1");
        string dummy = crewchangepercentageeditdummy.Text;
        if (checkbox.Checked == true)
        {

            crewchangepercentageedit.ReadOnly = false;
            crewchangepercentageedit.Text = dummy;
        }
        else
        {
            crewchangepercentageedit.ReadOnly = true;
            crewchangepercentageedit.Text = "";
        }

    }

    public void gvDrillJoblist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                GridEditableItem      item                                = e.Item as GridEditableItem;
                Guid?                 drilljobid                             = General.GetNullableGuid(item.GetDataKeyValue("FLDDRILLJOBID").ToString());
                int?                  vesselid                            = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());



                UserControlMaskNumber frequnecyedit                       = (UserControlMaskNumber)item.FindControl("tbfrequencyedit");
                UserControlHardExtn   frequencytypeedit                   = (UserControlHardExtn)item.FindControl("Radddlfrequencyedit");
                UserControlHardExtn   fixedorvariableedit                 = (UserControlHardExtn)item.FindControl("Radcbfixedorvariableedit");
                UserControlHardExtn   typeedit                            = (UserControlHardExtn)item.FindControl("Radddltypeedit");
                RadCheckBox           crewchangecheckbox                  = (RadCheckBox)item.FindControl("Radcheckcrewchangeedit");
                UserControlMaskNumber crewchangepercentage                = (UserControlMaskNumber)item.FindControl("tbcrewpercentedit");
                RadCheckBox           photoynedit                         = (RadCheckBox)item.FindControl("Radcbphotoynedit");
                RadCheckBox           dashboardynedit                     = (RadCheckBox)item.FindControl("Radcbdashboardynedit");


                DataTable              dt                                 = PhoenixInspectionDrillJob.drilljobeditlist(drilljobid);

                if (dt.Rows.Count > 0)
                {
                                      
                                      frequnecyedit.Text                  = dt.Rows[0]["FLDFREQUENCY"].ToString();
                                      frequencytypeedit.SelectedHard      = dt.Rows[0]["FLDFREQUENCYTYPEID"].ToString();
                                      fixedorvariableedit.SelectedHard    = dt.Rows[0]["FLDFIXEDORVARIABLEID"].ToString();
                                      typeedit.SelectedHard               = dt.Rows[0]["FLDTYPEID"].ToString();
                                      if (dt.Rows[0]["FLDAFFECTEDBYCREWCHANGEYN"].ToString() == "1")
                                       {
                                        crewchangecheckbox.Checked = true;
                                       }
                                      crewchangepercentage.Text = dt.Rows[0]["FLDCREWCHANGEPERCENTAGE"].ToString();
                                      if (dt.Rows[0]["FLDPHOTOYN"].ToString() == "1")
                                      {
                                          photoynedit.Checked = true;
                                      }
                                      if (dt.Rows[0]["FLDDASHBOARDYN"].ToString() == "1")
                                      {
                                          dashboardynedit.Checked = true;
                                      }

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;
        }
    }




     protected void gvDrillJoblist_ItemCommand(object sender, GridCommandEventArgs e)
     {

        

     }

    


    protected void gvDrillJoblist_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem            item                     = e.Item as GridEditableItem;

            UserControlMaskNumber       frequnecyedit                   = (UserControlMaskNumber)item.FindControl("tbfrequencyedit");
            UserControlHardExtn         frequencytypeedit               = (UserControlHardExtn)item.FindControl("Radddlfrequencyedit");
            UserControlHardExtn         fixedorvariableedit             = (UserControlHardExtn)item.FindControl("Radcbfixedorvariableedit");
            UserControlHardExtn         typeedit                        = (UserControlHardExtn)item.FindControl("Radddltypeedit");
            RadCheckBox                 crewchangecheckbox              = (RadCheckBox)item.FindControl("Radcheckcrewchangeedit");
            UserControlMaskNumber       crewchangepercentageedit        = (UserControlMaskNumber)item.FindControl("tbcrewpercentedit");
            RadCheckBox                 photoynedit                     = (RadCheckBox)item.FindControl("Radcbphotoynedit");
            RadCheckBox                 dashboardynedit                 = (RadCheckBox)item.FindControl("Radcbdashboardynedit");


            int                         rowusercode                     = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            Guid?                       drilljobid                      = General.GetNullableGuid(item.OwnerTableView.DataKeyValues[item.ItemIndex]["FLDDRILLJOBID"].ToString());
            int?                        frequency                       = General.GetNullableInteger(frequnecyedit.Text);
            int?                        frequencytypeid                 = General.GetNullableInteger(frequencytypeedit.SelectedHard);
            string                      frequencytype                   = string.Empty;
            if (frequencytypeid != null)
            {
                frequencytype = General.GetNullableString(frequencytypeedit.SelectedName);
            }
            int?                        fixedorvariableid               = General.GetNullableInteger(fixedorvariableedit.SelectedHard);
            string                      fixedorvariable                 = string.Empty;
            if (fixedorvariableid != null)
            {
                fixedorvariable = General.GetNullableString(fixedorvariableedit.SelectedName);
            }
            int?                        typeid                          = General.GetNullableInteger(typeedit.SelectedHard);
            string                      type                            = string.Empty;
            if (typeid !=null)
            {
                                        type                            = General.GetNullableString(typeedit.SelectedName);
            }
            int                         photoyn                         = (photoynedit.Checked) == true ? 1 : 0;
            int                         dashboardyn                     = (dashboardynedit.Checked) == true ? 1 : 0;
            int                         crewchangeyn                    = (crewchangecheckbox.Checked) == true ? 1 : 0;
            int?                        crewchangepercentage            = General.GetNullableInteger(crewchangepercentageedit.Text);

            if (!IsValidDrillJobDetails(frequency, frequencytype, frequencytypeid, crewchangeyn, crewchangepercentage,typeid, fixedorvariableid))
            {
                ucError.Visible = true;
                return;
            }


            PhoenixInspectionDrillJob.drilljobupdate(rowusercode 
                                                     ,drilljobid
                                                     ,frequency
                                                     ,frequencytypeid
                                                     ,frequencytype
                                                     ,photoyn
                                                     ,dashboardyn
                                                     ,fixedorvariable
                                                     ,fixedorvariableid
                                                     ,type
                                                     ,typeid
                                                     ,crewchangeyn
                                                     ,crewchangepercentage);

            PhoenixInspectionDrillJob.drillscheduleinsertwithdrilljobid(rowusercode, drilljobid);
            gvDrillJoblist.Rebind();

        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;
        }
    }

    private bool IsValidDrillJobDetails(int? frequency, string frequencytype , int? frequencytypeid , int crewchangeyn, int? crewpercentage ,  int? fixedorvariableid, int? typeid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (frequency == null)
        {
            ucError.ErrorMessage = "Frequency.";
        }
        if (frequencytypeid == null)
        {
            ucError.ErrorMessage = "Frequency type.";
        }

        if (frequencytype == "Months" && frequency > 12)
        {
            ucError.ErrorMessage = "Valid Frequency (1-12)";
        }

        if (frequencytype == "Weeks" && frequency > 52)
        {
            ucError.ErrorMessage = "Valid Frequency (1-52)";
        }

        if (crewchangeyn == 1 && crewpercentage == null)
        {
            ucError.ErrorMessage = "Crew Change Percentage";
        }

        if (fixedorvariableid == null)
        {
            ucError.ErrorMessage = "  Is Drill Fixed or Variable.";
        }
        if (typeid == null)
        {
            ucError.ErrorMessage = " Type of Drill.";
        }
        return (!ucError.IsError);
    }
}