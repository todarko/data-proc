use std::error::Error;
use std::process;
use serde::Deserialize;
use itertools::Itertools;
use csv::Writer;

#[derive(Deserialize, Clone, PartialEq, Hash, Eq)]
struct EmailRecord {
    email: String
}
#[derive(Deserialize, Clone, PartialEq, Hash, Eq)]
struct SignInRecord {
    email: String,
    ipaddress: String
}

fn process_file() -> Result<(), Box<dyn Error>> {
    use std::time::Instant;
    let now = Instant::now();

    // String vec so we can compare to string in SignInRecordType
    let mut email_list: Vec<String> = Vec::new();
    let mut phone_users: Vec<EmailRecord> = Vec::new();
    let mut phone_users_with_ip: Vec<SignInRecord> = Vec::new();
    let mut blah = 0;
    // Build the CSV reader and iterate over each record.
    //let mut rdr = csv::Reader::from_reader(io::stdin());
    let mut df = csv::Reader::from_path(".\\data\\csv\\DevicesWithInventoryUsernameOnly.csv")?;
    let mut sf = csv::Reader::from_path(".\\data\\csv\\InteractiveSignIns.csv")?;
    
    for record in df.deserialize() {
        let record: EmailRecord = record?;
        email_list.push(record.email.to_ascii_uppercase());
    }

    for record2 in sf.deserialize() {

        let mut record2: SignInRecord = record2?;
        record2.email = record2.email.to_ascii_uppercase();
        
        if record2.ipaddress == "1234.236.747.668" || record2.ipaddress == "5088.21.471.898" || record2.ipaddress == "1277.188.20.8202" {
            blah = blah + 1;
        } else if !email_list.contains(&record2.email) {
            let rec_clone = record2.clone();
            let rec_email = EmailRecord { email: rec_clone.email };
            phone_users_with_ip.push(record2);
            phone_users.push(rec_email);
            blah = blah + 1;
        } else {
            blah = blah + 1;
        }
    }
    println!("Total records processed: {}", blah);

    //println!("{}", phone_users.len());
    let unique_phone_users = phone_users.iter().cloned().unique().collect_vec();
    println!("{}", unique_phone_users.len());

    //println!("{}", phone_users_with_ip.len());
    let unique_phone_users_with_ip = phone_users_with_ip.iter().cloned().unique().collect_vec();
    println!("{}", unique_phone_users_with_ip.len());


    let mut wtr = Writer::from_path(".\\data\\output\\usersoutputrust.csv")?;
    for i in unique_phone_users {
        wtr.write_record(&[i.email])?;
    }
    wtr.flush()?;

    let mut wtr2 = Writer::from_path(".\\data\\output\\usersoutputwithiprust.csv")?;
    for i in unique_phone_users_with_ip {
        wtr2.write_record(&[i.email, i.ipaddress])?;
    }
    wtr2.flush()?;

    let elapsed = now.elapsed();
    println!("Elapsed Time Rusty: {:.2?}", elapsed);
    Ok(())
}

fn main() {

    if let Err(err) = process_file() {
        println!("error running example: {}", err);
        process::exit(1);
    }
}
