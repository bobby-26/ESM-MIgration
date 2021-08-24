using System;
using System.Configuration;
using System.Data;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web;
public partial class OptionsConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Login", "LOGIN");
        MenuConfig.MenuList = toolbar.Show();
        BindData();
        if(!IsPostBack)
        {
            lblManagement.Text = HttpContext.Current.Session["companyname"].ToString();
        }
    }
    protected void Config_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("LOGIN"))
        {
            Response.Redirect("~/Default.aspx");           
        }
    }
    private void BindData()
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("FLDKEY");
        dt.Columns.Add("FLDVALUE");
        Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
        AppSettingsSection AppSettings = (AppSettingsSection)configuration.GetSection("appSettings");
       
        string[] keys = AppSettings.Settings.AllKeys;
      
        for (int i = 0; i < keys.Length; i++)
        {           
            DataRow dr = dt.NewRow();
            dr["FLDKEY"] = keys[i];
            dr["FLDVALUE"] = AppSettings.Settings[keys[i]].Value;
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            gvList.DataSource = dt;
            gvList.DataBind();
        }
        else
        {
            
            ShowNoRecordsFound(dt, gvList);
        }      
    }
    protected void gvList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = e.NewEditIndex;
            BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            
            string key = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblKey")).Text;
            string value = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtValue")).Text;
            if(!IsValidatConfiguration(value))
            {
                ucError.Visible = true;
                return;
            }
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection webConfigSection = (AppSettingsSection)configuration.GetSection("appSettings");
            if (value != webConfigSection.Settings[key].Value)
            {
                webConfigSection.Settings.Remove(key);
                webConfigSection.Settings.Add(key, value);
                configuration.Save();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;        
        BindData();
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }
    private bool IsValidatConfiguration(string value)
    {
        ucError.HeaderMessage = "Please provide the following required information";
      
        if (string.IsNullOrEmpty(value.Trim()))
            ucError.ErrorMessage = "Value is required";

        return (!ucError.IsError);
    }
}
