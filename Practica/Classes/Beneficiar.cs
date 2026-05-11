using System;

namespace Practica.Classes
{
    public class Beneficiar
    {
        private int nrBen { get; set; }
        private string surname { get; set; }
        private string name { get; set; }
        private string phoneNumber { get; set; }
        private string email { get; set; }
        private string addres { get; set; }
        private int codLoc { get; set; }

        public Beneficiar()
        {

        }

        public Beneficiar(int nrBen, string surname, string name, string phoneNumber, string email, string addres, int codLoc)
        {
            this.nrBen = nrBen;
            this.surname = surname;
            this.name = name;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.addres = addres;
            this.codLoc = codLoc;
        }
        public bool IsEmpty()
        {
            return nrBen == 0 ||
                   string.IsNullOrWhiteSpace(surname) ||
                   string.IsNullOrWhiteSpace(name) ||
                   string.IsNullOrWhiteSpace(phoneNumber) ||
                   string.IsNullOrWhiteSpace(email) ||
                   string.IsNullOrWhiteSpace(addres) ||
                   codLoc == 0;
        }
    }
}