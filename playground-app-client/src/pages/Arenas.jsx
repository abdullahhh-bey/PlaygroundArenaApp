import ArenaCard from "../components/ArenaCard";

export default function Arenas() {
  const arenas = [
    {
      name: "Falcon Sports Arena",
      email: "contact@falconsports.com",
      location: "Lahore, Pakistan",
    },
    {
      name: "Falcon Sports Arena",
      email: "contact@falconsports.com",
      location: "Lahore, Pakistan",
    },
    {
      name: "Falcon Sports Arena",
      email: "contact@falconsports.com",
      location: "Lahore, Pakistan",
    },
    {
      name: "Falcon Sports Arena",
      email: "contact@falconsports.com",
      location: "Lahore, Pakistan",
    },
  ];

  return (
    <div className="container py-5">
      <h2
        className="text-center fw-bold mb-5 pt-4 pb-2"
        style={{ color: "#22305d" }}
      >
        Available Arenas
      </h2>
      <div className="row g-4">
        {arenas.map((arena, i) => (
          <div key={i} className="col-12 col-sm-6 col-md-4 col-lg-4 px-5">
            <ArenaCard {...arena} />
          </div>
        ))}
      </div>
    </div>
  );
}
