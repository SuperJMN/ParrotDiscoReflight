namespace ParrotDiscoReflight.ViewModels
{
    public class VirtualDashboard
    {
        public string Name { get; }

        public VirtualDashboard(string name)
        {
            Name = name;
        }

        public string Template
        {
            get { return Name; }
        }

        protected bool Equals(VirtualDashboard other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VirtualDashboard) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}