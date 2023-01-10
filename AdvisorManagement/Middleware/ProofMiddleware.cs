using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IdentityModel.Protocols.WSTrust;

namespace AdvisorManagement.Middleware
{
    public class ProofMiddleware
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        public object getListProof(string user_email, int id)
        {
            var role = db.AccountUser.FirstOrDefault(x=>x.email== user_email).id_role;
            var id_account = db.AccountUser.FirstOrDefault(x=>x.email== user_email).id;
            if(role == 2)
            {
                var listProof = (from pq in db.AccountUser
                                 join pr in db.ProofPlan on pq.id equals pr.id_creator
                                 where pr.id_titleplan == id && id_account == pr.id_creator 
                                 select new Models.ViewModel.ListProofPlan
                                 {
                                     id = pr.id,
                                     content = pr.content,
                                     proof = pr.file_proof,
                                     create_time = pr.create_time,
                                     semester = pr.semester,
                                     status = pr.status,
                                     creator = pq.user_name
                                 }).OrderByDescending(x => x.create_time).ToList();
                return listProof;
            } else if (role == 1)
            {
                var listProof = (from pq in db.AccountUser
                                 join pr in db.ProofPlan on pq.id equals pr.id_creator
                                 where pr.id_titleplan == id
                                 select new Models.ViewModel.ListProofPlan
                                 {
                                     id = pr.id,
                                     content = pr.content,
                                     proof = pr.file_proof,
                                     create_time = pr.create_time,
                                     semester = pr.semester,
                                     status = pr.status,
                                     creator = pq.user_name
                                 }).OrderByDescending(x=>x.create_time).ToList();
                return listProof;
            }
            
            return null;
        }
    }
}