// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace MonoDevelop.VersionControl {
    
    
    public partial class UrlBasedRepositoryEditor {
        
        private Gtk.Table table1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.ComboBox comboProtocol;
        
        private Gtk.HBox hbox2;
        
        private Gtk.SpinButton repositoryPortSpin;
        
        private Gtk.HSeparator hseparator1;
        
        private Gtk.HSeparator hseparator2;
        
        private Gtk.Label label11;
        
        private Gtk.Label label4;
        
        private Gtk.Label label5;
        
        private Gtk.Label label6;
        
        private Gtk.Label label7;
        
        private Gtk.Label label8;
        
        private Gtk.Label label9;
        
        private Gtk.Entry repositoryPassEntry;
        
        private Gtk.Entry repositoryPathEntry;
        
        private Gtk.Entry repositoryServerEntry;
        
        private Gtk.Entry repositoryUrlEntry;
        
        private Gtk.Entry repositoryUserEntry;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget MonoDevelop.VersionControl.UrlBasedRepositoryEditor
            Stetic.BinContainer.Attach(this);
            this.Events = ((Gdk.EventMask)(256));
            this.Name = "MonoDevelop.VersionControl.UrlBasedRepositoryEditor";
            // Container child MonoDevelop.VersionControl.UrlBasedRepositoryEditor.Gtk.Container+ContainerChild
            this.table1 = new Gtk.Table(((uint)(9)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            this.table1.BorderWidth = ((uint)(12));
            // Container child table1.Gtk.Table+TableChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            // Container child hbox1.Gtk.Box+BoxChild
            this.comboProtocol = Gtk.ComboBox.NewText();
            this.comboProtocol.Name = "comboProtocol";
            this.comboProtocol.Active = 0;
            this.hbox1.Add(this.comboProtocol);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox1[this.comboProtocol]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            this.table1.Add(this.hbox1);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table1[this.hbox1]));
            w2.TopAttach = ((uint)(2));
            w2.BottomAttach = ((uint)(3));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            // Container child hbox2.Gtk.Box+BoxChild
            this.repositoryPortSpin = new Gtk.SpinButton(0, 99999, 1);
            this.repositoryPortSpin.CanFocus = true;
            this.repositoryPortSpin.Name = "repositoryPortSpin";
            this.repositoryPortSpin.Adjustment.PageIncrement = 10;
            this.repositoryPortSpin.Adjustment.PageSize = 10;
            this.repositoryPortSpin.ClimbRate = 1;
            this.repositoryPortSpin.Numeric = true;
            this.repositoryPortSpin.Value = 1;
            this.hbox2.Add(this.repositoryPortSpin);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox2[this.repositoryPortSpin]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            this.table1.Add(this.hbox2);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.hbox2]));
            w4.TopAttach = ((uint)(4));
            w4.BottomAttach = ((uint)(5));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(2));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.hseparator1 = new Gtk.HSeparator();
            this.hseparator1.Name = "hseparator1";
            this.table1.Add(this.hseparator1);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.hseparator1]));
            w5.TopAttach = ((uint)(6));
            w5.BottomAttach = ((uint)(7));
            w5.RightAttach = ((uint)(2));
            w5.YPadding = ((uint)(6));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.hseparator2 = new Gtk.HSeparator();
            this.hseparator2.Name = "hseparator2";
            this.table1.Add(this.hseparator2);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table1[this.hseparator2]));
            w6.TopAttach = ((uint)(1));
            w6.BottomAttach = ((uint)(2));
            w6.RightAttach = ((uint)(2));
            w6.YPadding = ((uint)(6));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.Xalign = 0F;
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("Server:");
            this.table1.Add(this.label11);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table1[this.label11]));
            w7.TopAttach = ((uint)(3));
            w7.BottomAttach = ((uint)(4));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Url:");
            this.table1.Add(this.label4);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table1[this.label4]));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xalign = 0F;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Protocol:");
            this.table1.Add(this.label5);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table1[this.label5]));
            w9.TopAttach = ((uint)(2));
            w9.BottomAttach = ((uint)(3));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.Xalign = 0F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Port:");
            this.table1.Add(this.label6);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table1[this.label6]));
            w10.TopAttach = ((uint)(4));
            w10.BottomAttach = ((uint)(5));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Path:");
            this.table1.Add(this.label7);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table1[this.label7]));
            w11.TopAttach = ((uint)(5));
            w11.BottomAttach = ((uint)(6));
            w11.XOptions = ((Gtk.AttachOptions)(4));
            w11.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("User:");
            this.table1.Add(this.label8);
            Gtk.Table.TableChild w12 = ((Gtk.Table.TableChild)(this.table1[this.label8]));
            w12.TopAttach = ((uint)(7));
            w12.BottomAttach = ((uint)(8));
            w12.XOptions = ((Gtk.AttachOptions)(4));
            w12.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.Xalign = 0F;
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("Password:");
            this.table1.Add(this.label9);
            Gtk.Table.TableChild w13 = ((Gtk.Table.TableChild)(this.table1[this.label9]));
            w13.TopAttach = ((uint)(8));
            w13.BottomAttach = ((uint)(9));
            w13.XOptions = ((Gtk.AttachOptions)(4));
            w13.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.repositoryPassEntry = new Gtk.Entry();
            this.repositoryPassEntry.CanFocus = true;
            this.repositoryPassEntry.Name = "repositoryPassEntry";
            this.repositoryPassEntry.IsEditable = true;
            this.repositoryPassEntry.Visibility = false;
            this.repositoryPassEntry.InvisibleChar = '●';
            this.table1.Add(this.repositoryPassEntry);
            Gtk.Table.TableChild w14 = ((Gtk.Table.TableChild)(this.table1[this.repositoryPassEntry]));
            w14.TopAttach = ((uint)(8));
            w14.BottomAttach = ((uint)(9));
            w14.LeftAttach = ((uint)(1));
            w14.RightAttach = ((uint)(2));
            w14.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.repositoryPathEntry = new Gtk.Entry();
            this.repositoryPathEntry.CanFocus = true;
            this.repositoryPathEntry.Name = "repositoryPathEntry";
            this.repositoryPathEntry.IsEditable = true;
            this.repositoryPathEntry.InvisibleChar = '●';
            this.table1.Add(this.repositoryPathEntry);
            Gtk.Table.TableChild w15 = ((Gtk.Table.TableChild)(this.table1[this.repositoryPathEntry]));
            w15.TopAttach = ((uint)(5));
            w15.BottomAttach = ((uint)(6));
            w15.LeftAttach = ((uint)(1));
            w15.RightAttach = ((uint)(2));
            w15.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.repositoryServerEntry = new Gtk.Entry();
            this.repositoryServerEntry.CanFocus = true;
            this.repositoryServerEntry.Name = "repositoryServerEntry";
            this.repositoryServerEntry.IsEditable = true;
            this.repositoryServerEntry.InvisibleChar = '●';
            this.table1.Add(this.repositoryServerEntry);
            Gtk.Table.TableChild w16 = ((Gtk.Table.TableChild)(this.table1[this.repositoryServerEntry]));
            w16.TopAttach = ((uint)(3));
            w16.BottomAttach = ((uint)(4));
            w16.LeftAttach = ((uint)(1));
            w16.RightAttach = ((uint)(2));
            w16.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.repositoryUrlEntry = new Gtk.Entry();
            this.repositoryUrlEntry.CanFocus = true;
            this.repositoryUrlEntry.Name = "repositoryUrlEntry";
            this.repositoryUrlEntry.IsEditable = true;
            this.repositoryUrlEntry.InvisibleChar = '●';
            this.table1.Add(this.repositoryUrlEntry);
            Gtk.Table.TableChild w17 = ((Gtk.Table.TableChild)(this.table1[this.repositoryUrlEntry]));
            w17.LeftAttach = ((uint)(1));
            w17.RightAttach = ((uint)(2));
            w17.YOptions = ((Gtk.AttachOptions)(0));
            // Container child table1.Gtk.Table+TableChild
            this.repositoryUserEntry = new Gtk.Entry();
            this.repositoryUserEntry.CanFocus = true;
            this.repositoryUserEntry.Name = "repositoryUserEntry";
            this.repositoryUserEntry.IsEditable = true;
            this.repositoryUserEntry.InvisibleChar = '●';
            this.table1.Add(this.repositoryUserEntry);
            Gtk.Table.TableChild w18 = ((Gtk.Table.TableChild)(this.table1[this.repositoryUserEntry]));
            w18.TopAttach = ((uint)(7));
            w18.BottomAttach = ((uint)(8));
            w18.LeftAttach = ((uint)(1));
            w18.RightAttach = ((uint)(2));
            w18.YOptions = ((Gtk.AttachOptions)(0));
            this.Add(this.table1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.repositoryUserEntry.Changed += new System.EventHandler(this.OnRepositoryUserEntryChanged);
            this.repositoryUrlEntry.Changed += new System.EventHandler(this.OnRepositoryUrlEntryChanged);
            this.repositoryServerEntry.Changed += new System.EventHandler(this.OnRepositoryServerEntryChanged);
            this.repositoryPathEntry.Changed += new System.EventHandler(this.OnRepositoryPathEntryChanged);
            this.repositoryPassEntry.Changed += new System.EventHandler(this.OnRepositoryPassEntryChanged);
            this.repositoryPortSpin.Changed += new System.EventHandler(this.OnRepositoryPortSpinChanged);
            this.comboProtocol.Changed += new System.EventHandler(this.OnComboProtocolChanged);
        }
    }
}
